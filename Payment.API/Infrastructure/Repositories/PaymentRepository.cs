using Microsoft.EntityFrameworkCore;
using Payment.API.Domain;
using Payment.API.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Payment.API.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public PaymentRepository(PaymentContext context)
        {
            _context = context;
        }

        public async Task<PaymentRequest> GetAsync(Guid paymentId)
        {
            var payment = await _context.PaymentRequests
                .FirstOrDefaultAsync(a => a.Id == paymentId);

            return payment;
        }

    }
}
