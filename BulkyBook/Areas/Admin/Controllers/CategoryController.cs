using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyBook.Areas.Category.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            BulkyBook.Models.Category category = new BulkyBook.Models.Category(); //SOME ERROR with Single class initialization.
            if(id == null)
            {
                // Create
                return View(category);
            }
            // Update
            category = _categoryRepository.GetById(id.GetValueOrDefault());
            if(category == null)
            {
                // Incorrect ID
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BulkyBook.Models.Category category)
        {
            if (ModelState.IsValid)
            {
                if(category.Id == 0)
                {
                    _categoryRepository.Add(category);
                }
                else
                {
                    _categoryRepository.Update(category);
                }
                _categoryRepository.Commit();
                return RedirectToAction(nameof(Index)); // action name
            }
            return View(category);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _categoryRepository.GetAll();
            return Json(new { data = categories });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objToDelete = _categoryRepository.GetById(id);
            if(objToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting a record" });
            }
            _categoryRepository.Remove(id);
            _categoryRepository.Commit();
            return Json(new { success = true, message = "Record Deleted Successfully" });
        }
        #endregion
    }
}
