using Microsoft.EntityFrameworkCore;
using Payment.API.Domain;
using Payment.API.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Payment.API.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly PaymentContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public AccountRepository(PaymentContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAsync(Guid accountId)
        {
            var account = await _context.Accounts
                .Include(a => a.PaymentRequests)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            return account;
        }

        public void Update(Account account)
        {
            _context.Entry(account).State = EntityState.Modified;
        }
    }
}
