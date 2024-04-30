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

<<<<<<< HEAD
=======
        public ICompanyRepository CompanyRepository { get; private set; }

        public IShoppingRepository ShoppingRepository { get; private set; }

        public IOrderHeaderRepository OrderHeaderRepository { get; private set; }

        public IOrderDetailRepository OrderDetailRepository { get; private set; }

        public IApplicationUserRepository ApplicationUserRepository { get; private set; }


>>>>>>> swarnajit
        private ApplicationDBContext _db;

        public UnitOfWork(ApplicationDBContext db)
        {
            _db = db;
            CategoryRepository = new CategoryRepository(_db);
            ProductRepository = new ProductRepository(_db);
<<<<<<< HEAD
=======
            CompanyRepository = new CompanyRepository(_db);
            ShoppingRepository = new ShoppingCartRepository(_db);
            OrderDetailRepository = new OrderDetailRepository(_db);
            OrderHeaderRepository = new OrderHaderRepository(_db);
            ApplicationUserRepository = new ApplicationUserRepository(_db);

>>>>>>> swarnajit
        }
        public void save()
        {
            _db.SaveChanges();
        }
    }
}
