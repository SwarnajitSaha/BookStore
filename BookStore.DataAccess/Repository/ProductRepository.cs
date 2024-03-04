using BookStore.Data;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>,IProductRepository
    {
        public readonly ApplicationDBContext _db;
        public ProductRepository(ApplicationDBContext db) : base(db)         {
            _db = db;
        }

        public void update(Product product)
        {
            var objFromDb = _db.ProductTable.FirstOrDefault(u => u.Id== product.Id);
            if (objFromDb != null)
            {
                if(product.Title != null) objFromDb.Title = product.Title;
                if(product.Description != null) objFromDb.Description = product.Description;
                if(product.Category != null) objFromDb.Category = product.Category;
                if(product.ListPrice!= 0) objFromDb.ListPrice = product.ListPrice;
                if(product.Price != 0) objFromDb.Price = product.Price;
                if(product.Price50!= 0) objFromDb.Price50 = product.Price50;
                if(product.Price100!=0) objFromDb.Price100 = product.Price100;
                if(product.ImageUrl!=null) objFromDb.ImageUrl = product.ImageUrl;  

            }
        }
    }
}
