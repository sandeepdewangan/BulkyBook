using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using BulkyBook.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

 
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _dbContext.ApplicationUsers.Include(u => u.Company).ToList() ;
            var userRoles = _dbContext.UserRoles.ToList();
            var roles = _dbContext.Roles.ToList();

            foreach(var user in userList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if(user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }

            return Json(new { data = userList });
        }

        //[HttpDelete]
        //public IActionResult Delete(int id)
        //{
        //    var objToDelete = _categoryRepository.GetById(id);
        //    if(objToDelete == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting a record" });
        //    }
        //    _categoryRepository.Remove(id);
        //    _categoryRepository.Commit();
        //    return Json(new { success = true, message = "Record Deleted Successfully" });
        //}
        #endregion
    }
}
