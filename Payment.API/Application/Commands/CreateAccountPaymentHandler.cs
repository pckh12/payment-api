using MediatR;
using Payment.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.API.Application.Commands
{
    public class CreateAccountPaymentHandler : IRequestHandler<CreateAccountPaymentCommand, Guid>
    {
        private readonly IAccountRepository _accountRespository;
        public CreateAccountPaymentHandler(IAccountRepository accountRespository)
        {
            _accountRespository = accountRespository;
        }

        public async Task<Guid> Handle(CreateAccountPaymentCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRespository.GetAsync(request.AccountId);

            if (account == null)
            {
                throw new PaymentDomainException("Account is not valid");
            }

            var paymentRequestId = account.AddPaymentRequest(request.Command.Date, request.Command.Amount);
            _accountRespository.Update(account);
            await _accountRespository.UnitOfWork.SaveEntitiesAsync();

            return paymentRequestId;
        }
    }
}
