using BookStore.Data;
using BookStore.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
  

        public ICategoryRepository CategoryRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public ICompanyRepository CompanyRepository { get; private set; }

        public IShoppingRepository ShoppingRepository { get; private set; }

        private ApplicationDBContext _db;

        public UnitOfWork(ApplicationDBContext db)
        {
            _db = db;
            CategoryRepository = new CategoryRepository(_db);
            ProductRepository = new ProductRepository(_db);
            CompanyRepository = new CompanyRepository(_db);
            ShoppingRepository = new ShoppingCartRepository(_db);
        }
        public void save()
        {
            _db.SaveChanges();
        }
    }
}
