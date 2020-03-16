using Payment.API.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Payment.API.Domain
{
    public interface IAccountRepository
    {
        Task<Account> GetAsync(Guid accountId);

        void Update(Account account);

        IUnitOfWork UnitOfWork { get; }
    }
}
