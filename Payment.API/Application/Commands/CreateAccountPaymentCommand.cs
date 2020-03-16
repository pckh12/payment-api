using MediatR;
using System;

namespace Payment.API.Application.Commands
{
    public class CreateAccountPaymentCommand : IRequest<Guid>
    {
        public Guid AccountId { get; }
        public CreatePaymentCommand Command { get; }

        public CreateAccountPaymentCommand(Guid accountId, CreatePaymentCommand createPaymentCommand)
        {
            AccountId = accountId;
            Command = createPaymentCommand;
        }
    }
}
