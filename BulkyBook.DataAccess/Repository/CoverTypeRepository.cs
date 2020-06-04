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
    public class CoverTypeRepository : ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public CoverTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(CoverType cover)
        {
            _db.Add(cover);
        }

        public void Commit()
        {
            _db.SaveChanges();
        }

        public IEnumerable<CoverType> GetAll()
        {
            var cover = _db.Covers.ToList();
            return cover;
        }

        public CoverType GetById(int id)
        {
            return _db.Covers.Find(id);
        }

        public void Remove(int id)
        {
            CoverType coverToDelete = _db.Covers.Find(id);
            _db.Covers.Remove(coverToDelete);
        }

        public void Update(CoverType cover)
        {
            var obj = _db.Covers.FirstOrDefault(s => s.Id == cover.Id);
            if (obj != null)
            {
                obj.Name = cover.Name;
            }
        }
    }
}
