using MediatR;
using Payment.API.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.API.Application.Queries
{
    public class GetPaymentsHandler : IRequestHandler<GetPaymentsQuery, AccountViewModel>
    {
        private readonly IAccountRepository _accountRespository;
        public GetPaymentsHandler(IAccountRepository accountRespository)
        {
            _accountRespository = accountRespository;
        }

        public async Task<AccountViewModel> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRespository.GetAsync(request.AccountId);
            if (account == null)
            {
                throw new PaymentDomainException("Account is not valid");
            }
            
            var accountDTO = AccountViewModel.FromAccount(account);
            return accountDTO;
        }
 
    }
}
