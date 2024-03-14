using BookStore.Data;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository.IRepository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingRepository
    {
        public readonly ApplicationDBContext _db;

        public ShoppingCartRepository(ApplicationDBContext db) : base(db) 
        {
            _db = db;
        }
        
        public void update(ShoppingCart cart)
        {
            _db.ShoppingCarts.Update(cart);
        }


    }
}
