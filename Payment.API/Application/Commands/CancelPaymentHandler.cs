using Payment.API.Domain;
using Payment.API.Domain.Model;

namespace Payment.API.Application.Commands
{
    public class CancelPaymentHandler : ChangePaymentStatusHandler<CancelPaymentCommand>
    {
        public CancelPaymentHandler(IAccountRepository accountRespository, IPaymentRepository paymentRespository)
            : base(accountRespository, paymentRespository)
        {
        }

        public override void ChangePaymentStatus(PaymentRequest payment, string reason)
        {
            payment.CancelPayment(reason);
        }
    }
}
