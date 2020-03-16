using System;

namespace Payment.API.Application.Commands
{
    public class CancelPaymentCommand : ChangePaymentStatusCommand
    {
        public CancelPaymentCommand(Guid accountId, Guid paymentId, string reason) : base(accountId, paymentId, reason)
        {  
        }
    }
}
