using MediatR;
using Payment.API.Domain;
using Payment.API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.API.Application.Commands
{
    public abstract class ChangePaymentStatusHandler<TCommand> : IRequestHandler<TCommand, bool>
        where TCommand: ChangePaymentStatusCommand
    {
        private readonly IAccountRepository _accountRespository;
        private readonly IPaymentRepository _paymentRespository;

        public ChangePaymentStatusHandler(IAccountRepository accountRespository, IPaymentRepository paymentRespository)
        {
            _accountRespository = accountRespository;
            _paymentRespository = paymentRespository;
        }

        public async Task<bool> Handle(TCommand request, CancellationToken cancellationToken)
        {
            await EnsureAccountIsValid(request.AccountId);

            var payment = await _paymentRespository.GetAsync(request.PaymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException();
            }

            ChangePaymentStatus(payment, request.Reason);

            return await _paymentRespository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }

        public abstract void ChangePaymentStatus(PaymentRequest payment, string reason);

        private async Task<bool> EnsureAccountIsValid(Guid accountId)
        {
            var account = await _accountRespository.GetAsync(accountId);

            if (account == null)
            {
                throw new PaymentDomainException("Account is not valid");
            }

            return true;
        }
    }
}
