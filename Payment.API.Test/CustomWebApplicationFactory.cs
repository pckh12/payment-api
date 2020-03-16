using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Payment.API.Infrastructure;

namespace Payment.API.Test
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add in-memory database context 
                services.AddDbContext<PaymentContext>(options =>
                {
                    options.UseInMemoryDatabase("Payment");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var dbContext = scopedServices.GetRequiredService<PaymentContext>();

                    dbContext.Database.EnsureCreated();

                    // Seed test data
                    PopulateTestData(dbContext);
                }
            });
        }

        private void PopulateTestData(PaymentContext dbContext)
        {
           // dbContext.Accounts.Add()
        }
    }
}
