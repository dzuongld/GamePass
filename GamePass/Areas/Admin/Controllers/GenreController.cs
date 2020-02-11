using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePass.Models;
using GamePass.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace GamePass.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Genre genre = new Genre();
            if (id == null)
            {
                //create
                return View(genre);
            }

            genre = _unitOfWork.Genre.Get(id.GetValueOrDefault());
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Genre genre)
        {
            // double validation, as validation is already included in js
            if (ModelState.IsValid)
            {
                if (genre.Id == 0)
                {
                    _unitOfWork.Genre.Add(genre);

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
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Genre.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objDb = _unitOfWork.Genre.Get(id);
            if (objDb == null)
            {
                return Json(new { success = false, message = "Error deleting" });
            }
            _unitOfWork.Genre.Remove(objDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}