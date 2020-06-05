using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IProductRepository
    {
           IEnumerable<Product> GetAll();
           IEnumerable<Product> GetAllWithAllFKRef();
           Product GetById(int id);
           void Add(Product product);
           void Update(Product product);
           void Commit();
           void Remove(int id);
    }
}
