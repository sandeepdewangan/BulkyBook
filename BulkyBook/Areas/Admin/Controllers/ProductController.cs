using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using BulkyBook.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace BulkyBook.Areas.Product.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICoverTypeRepository _coverTypeRepository;
        private readonly IWebHostEnvironment _hostEnvironment; // For Image upload to wwwroot.

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, ICoverTypeRepository coverTypeRepository, IWebHostEnvironment hostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _coverTypeRepository = coverTypeRepository;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductViewModel productVM = new ProductViewModel()
            {
                Product = new BulkyBook.Models.Product(),
                CategoryList = _categoryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
                CoverTypeList = _coverTypeRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            BulkyBook.Models.Product product = new BulkyBook.Models.Product(); //SOME ERROR with Single class initialization.
            if(id == null)
            {
                // Create
                return View(productVM);
            }
            // Update
            productVM.Product = _productRepository.GetById(id.GetValueOrDefault());
            if(productVM.Product == null)
            {
                // Incorrect ID
                return NotFound();
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                // for image upload we need root path. 
                // images will be saved under root/images/products
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products");
                    var extension = Path.GetExtension(files[0].FileName);

                    if(productVM.Product.ImageUrl != null)
                    {
                        // this is update
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using(var fileStreams = new FileStream(Path.Combine(uploads, fileName+extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                else
                {
                    //update when they do not change the image
                    if(productVM.Product.Id != 0)
                    {
                        BulkyBook.Models.Product objFromDb = _productRepository.GetById(productVM.Product.Id);
                        productVM.Product.ImageUrl = objFromDb.ImageUrl;
                    }
                }

                if(productVM.Product.Id == 0)
                {
                    _productRepository.Add(productVM.Product);
                }
                else
                {
                    _productRepository.Update(productVM.Product);
                }
                _productRepository.Commit();
                return RedirectToAction(nameof(Index)); // action name
            }
            return View(productVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productRepository.GetAllWithAllFKRef();
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objToDelete = _productRepository.GetById(id);
            if(objToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting a record" });
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objToDelete.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _productRepository.Remove(id);
            _productRepository.Commit();
            return Json(new { success = true, message = "Record Deleted Successfully" });
        }
        #endregion
    }
}
