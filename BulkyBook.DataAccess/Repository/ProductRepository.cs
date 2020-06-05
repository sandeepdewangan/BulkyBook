using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository 
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(Product product)
        {
            _db.Add(product);
        }

        public void Commit()
        {
            _db.SaveChanges();
        }

        public IEnumerable<Product> GetAll()
        {
            var product = _db.Products.ToList();
            return product;
        }

        public IEnumerable<Product> GetAllWithAllFKRef()
        {
            var product = _db.Products.Include("Category").Include("CoverType").ToList();
            return product;
        }

        public Product GetById(int id)
        {
            return _db.Products.Find(id);
        }

        public void Remove(int id)
        {
            Product productToDelete = _db.Products.Find(id);
            _db.Products.Remove(productToDelete);
        }

        public void Update(Product product)
        {
            var obj = _db.Products.FirstOrDefault(s => s.Id == product.Id);
            if (obj != null)
            {
                if(product.ImageUrl != null)
                {
                    obj.ImageUrl = product.ImageUrl;
                }

                obj.Title = product.Title;
                obj.Description = product.Description;
                obj.ISBN = product.ISBN;
                obj.Author = product.Author;
                obj.ListPrice = product.ListPrice;
                obj.CategoryId = product.CategoryId;
                obj.CoverTypeId = product.CoverTypeId;
            }
        }
    }
}
