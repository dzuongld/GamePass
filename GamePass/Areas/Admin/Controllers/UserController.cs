using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePass.Data;
using GamePass.Models;
using GamePass.Repository.IRepository;
using GamePass.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GamePass.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
                
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.ApplicationUsers.ToList();

            // get roles from database and map to user's role
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                
            }

            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objDb == null)
            {
                return Json(new { success = false, message = "Error Locking/Unlocking" });
            }

            if (objDb.LockoutEnd != null && objDb.LockoutEnd > DateTime.Now)
            {
                // locked => will be unlocked
                objDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objDb.LockoutEnd = DateTime.Now.AddYears(10);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Locking/Unlocking Successful" });
        }

        #endregion
    }
}