using BookStore.Data;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class OrderHaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        public readonly ApplicationDBContext _db;
        public OrderHaderRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void UpadateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeadersTable.FirstOrDefault(i => i.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void update(OrderHeader orderHeader)
        {
            _db.OrderHeadersTable.Update(orderHeader);
        }

        public void UpdateScriptPaymentId(int id, string sessionId, string paymentIntentId)
        {
            var orderFromDb = _db.OrderHeadersTable.FirstOrDefault(i => i.Id == id);
            if (orderFromDb != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    orderFromDb.SessionId= sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    orderFromDb.PaymentInternId= paymentIntentId;
                    orderFromDb.PaymentDate= DateTime.Now;  
                }
            }
        }
    }
}
