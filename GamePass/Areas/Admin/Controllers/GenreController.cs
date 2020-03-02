using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePass.Models;
using GamePass.Models.ViewModels;
using GamePass.Repository.IRepository;
using GamePass.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamePass.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class GenreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(int productPage = 1)
        {
            GenreViewModel genreVM = new GenreViewModel()
            {
                Genres = await _unitOfWork.Genre.GetAllAsync()
            };

            var count = genreVM.Genres.Count();
            genreVM.Genres = genreVM.Genres.OrderBy(p => p.Name)
                .Skip((productPage - 1) * 2).Take(2).ToList();

            genreVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = 2,
                TotalItem = count,
                UrlParam = "/Admin/Genre/Index?productPage=:"
            };

            return View(genreVM);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Genre genre = new Genre();
            if (id == null)
            {
                //create
                return View(genre);
            }

            genre = await _unitOfWork.Genre.GetAsync(id.GetValueOrDefault());
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Genre genre)
        {
            // double validation, as validation is already included in js
            if (ModelState.IsValid)
            {
                if (genre.Id == 0)
                {
                    await _unitOfWork.Genre.AddAsync(genre);

                }
                else
                {
                    _unitOfWork.Genre.Update(genre);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index)); // avoid magic string "Index"
            }
            return View(genre);
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.Genre.GetAllAsync();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objDb = await _unitOfWork.Genre.GetAsync(id);
            if (objDb == null)
            {
                TempData["Error"] = "Error deleting Genre";
                return Json(new { success = false, message = "Error deleting" });
            }
            await _unitOfWork.Genre.RemoveAsync(objDb);
            _unitOfWork.Save();
            TempData["Success"] = "Genre deleted successfully";
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}