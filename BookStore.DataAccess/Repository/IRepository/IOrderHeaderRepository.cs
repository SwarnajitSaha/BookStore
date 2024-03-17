using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void update(OrderHeader orderHeader);
        void UpadateStatus(int id, string orderStatus, String? paymentStatus=null);
        void UpdateScriptPaymentId(int id, string sessionId, string paymentIntentId);
    }
}
