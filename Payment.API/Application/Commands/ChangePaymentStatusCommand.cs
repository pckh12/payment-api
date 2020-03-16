using MediatR;
using System;

namespace Payment.API.Application.Commands
{
    public abstract class ChangePaymentStatusCommand : IRequest<bool>
    {
        public Guid PaymentId { get; }
        public string Reason { get; }

        public Guid AccountId { get; }

        public ChangePaymentStatusCommand(Guid accountId, Guid paymentId, string reason)
        {
            PaymentId = paymentId;
            Reason = reason;
            AccountId = accountId;
        }
    }
}
