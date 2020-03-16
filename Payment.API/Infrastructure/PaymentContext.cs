using Microsoft.EntityFrameworkCore;
using Payment.API.Domain;
using Payment.API.Infrastructure.EntityConfigurations;
using Payment.API.Domain.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.API.Infrastructure
{
    public class PaymentContext : DbContext, IUnitOfWork
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<PaymentRequest> PaymentRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AccountEntityTypeConfiguration());
            builder.ApplyConfiguration(new PaymentRequestEntityTypeConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
