using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GamePass.Models;
using GamePass.Repository.IRepository;
using GamePass.Utility;
using Microsoft.AspNetCore.Mvc;

namespace GamePass.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlatformController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlatformController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Platform platform = new Platform();
            if (id == null)
            {
                //create
                return View(platform);
            }

            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            platform = _unitOfWork.SP_Call.OneRecord<Platform>(StaticDetails.Proc_Platform_Get, parameter);

            if (platform == null)
            {
                return NotFound();
            }

            return View(platform);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Platform platform)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Name", platform.Name);

            // double validation, as validation is already included in js
            if (ModelState.IsValid)
            {
                if (platform.Id == 0)
                {
                    _unitOfWork.SP_Call.Execute(StaticDetails.Proc_Platform_Create, parameter);
                }
                else
                {
                    parameter.Add("@Id", platform.Id);
                    _unitOfWork.SP_Call.Execute(StaticDetails.Proc_Platform_Update, parameter);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index)); // avoid magic string "Index"
            }
            return View(platform);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<Platform>(StaticDetails.Proc_Platform_GetAll, null);
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            var objDb = _unitOfWork.SP_Call.OneRecord<Platform>(StaticDetails.Proc_Platform_Get, parameter);

            if (objDb == null)
            {
                return Json(new { success = false, message = "Error deleting" });
            }

            _unitOfWork.SP_Call.Execute(StaticDetails.Proc_Platform_Delete, parameter);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}