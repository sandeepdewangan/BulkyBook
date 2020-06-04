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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(Category category)
        {
            _db.Add(category);
        }

        public void Commit()
        {
            _db.SaveChanges();
        }

        public IEnumerable<Category> GetAll()
        {
            var categories = _db.Categories.ToList();
            return categories;
        }

        public Category GetById(int id)
        {
            return _db.Categories.Find(id);
        }

        public void Remove(int id)
        {
            Category categoryToDelete = _db.Categories.Find(id);
            _db.Categories.Remove(categoryToDelete);
        }

        public void Update(Category category)
        {
            var obj = _db.Categories.FirstOrDefault(s => s.Id == category.Id);
            if (obj != null)
            {
                obj.Name = category.Name;
            }
        }
    }
}
