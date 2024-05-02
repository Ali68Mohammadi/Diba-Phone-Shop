using DibaEShop.DataAccess.Data;
using DibaEShop.DataAccess.Repository.IRepository;
using DibaEShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DibaEShop.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public OrderHeader GetLastOrderByUserId(string userId)
        {
            var lastOrderHeader = _context.OrderHeader
                .Where(o => o.ApplicatioUserId == userId)
                .OrderByDescending(item=>item.Id)
                .FirstOrDefault();
            return lastOrderHeader;
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeader.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _context.OrderHeader.FirstOrDefault(o => o.Id == id);

            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }

            }
            _context.SaveChanges();
        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentItentId)
        {
            var orderFromDb = _context.OrderHeader.FirstOrDefault(o => o.Id == id);
            orderFromDb.SessionId = sessionId;
            orderFromDb.PaymentIntenId = paymentItentId;
        }
    }
}
