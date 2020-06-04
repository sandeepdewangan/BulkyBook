using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface ICoverTypeRepository
    {
        IEnumerable<CoverType> GetAll();
        CoverType GetById(int id);
        void Add(CoverType cover);
        void Update(CoverType cover);
        void Commit();
        void Remove(int id);
    }
}
