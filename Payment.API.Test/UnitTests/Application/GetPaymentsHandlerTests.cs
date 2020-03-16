using Moq;
using Payment.API.Application.Queries;
using Payment.API.Domain;
using Payment.API.Domain.Model;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace Payment.API.Test.UnitTests.Application
{
    public class GetPaymentsHandlerTests
    {
        [Fact]
        public async void Handle_ShouldReturnViewModel()
        {
            var accountsRepo = new Mock<IAccountRepository>();
            var account = new Account(Guid.NewGuid(), 100);
            account.AddPaymentRequest(new DateTime(2020, 3, 1), 40);

            accountsRepo.Setup(a => a.GetAsync(account.Id)).ReturnsAsync(account);

            var handler = new GetPaymentsHandler(accountsRepo.Object);

            var query = new GetPaymentsQuery(account.Id);
            var viewModel = await handler.Handle(query, new CancellationToken());

            Assert.Equal(account.Balance, viewModel.Balance);
            Assert.Equal(account.PaymentRequests.First().Amount, viewModel.Payments.First().Amount);
            Assert.Equal(account.PaymentRequests.First().Date, viewModel.Payments.First().Date);
            Assert.Equal(account.PaymentRequests.First().Reason, viewModel.Payments.First().Reason);
            Assert.Equal(account.PaymentRequests.First().Status.ToString(), viewModel.Payments.First().Status);
        }
    }
}
