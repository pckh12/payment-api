using Payment.API.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Payment.API.Domain
{
    public interface IPaymentRepository
    {
        Task<PaymentRequest> GetAsync(Guid paymentId);

        IUnitOfWork UnitOfWork { get; }
    }
}
