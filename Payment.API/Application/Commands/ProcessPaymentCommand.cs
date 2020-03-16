using System;

namespace Payment.API.Application.Commands
{
    public class ProcessPaymentCommand : ChangePaymentStatusCommand
    {
        public ProcessPaymentCommand(Guid accountId, Guid paymentId) : base(accountId, paymentId, null)
        {
        }
    }
}
