using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyBook.Areas.CoverType.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly ICoverTypeRepository _coverTypeRepository;

        public CoverTypeController(ICoverTypeRepository coverTypeRepository)
        {
            _coverTypeRepository = coverTypeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            BulkyBook.Models.CoverType cover = new BulkyBook.Models.CoverType(); //SOME ERROR with Single class initialization.
            if(id == null)
            {
                // Create
                return View(cover);
            }
            // Update
            cover = _coverTypeRepository.GetById(id.GetValueOrDefault());
            if(cover == null)
            {
                // Incorrect ID
                return NotFound();
            }
            return View(cover);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BulkyBook.Models.CoverType cover)
        {
            if (ModelState.IsValid)
            {
                if(cover.Id == 0)
                {
                    _coverTypeRepository.Add(cover);
                }
                else
                {
                    _coverTypeRepository.Update(cover);
                }
                _coverTypeRepository.Commit();
                return RedirectToAction(nameof(Index)); // action name
            }
            return View(cover);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _coverTypeRepository.GetAll();
            return Json(new { data = categories });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objToDelete = _coverTypeRepository.GetById(id);
            if(objToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting a record" });
            }
            _coverTypeRepository.Remove(id);
            _coverTypeRepository.Commit();
            return Json(new { success = true, message = "Record Deleted Successfully" });
        }
        #endregion
    }
}
