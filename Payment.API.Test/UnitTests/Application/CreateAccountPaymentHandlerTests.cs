using Moq;
using Payment.API.Application.Commands;
using Payment.API.Domain;
using Payment.API.Domain.Model;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace Payment.API.Test.UnitTests.Application
{
    public class CreateAccountPaymentHandlerTests
    {
        [Fact]
        public async void Handle_ShouldCreatePendingPayment()
        {
            var accountsRepo = new Mock<IAccountRepository>();

            var account = new Account(Guid.NewGuid(), 100);

            accountsRepo.Setup(a => a.GetAsync(account.Id)).ReturnsAsync(account);
            accountsRepo.Setup(p => p.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var amount = 10;
            var date = new DateTime(2020, 3, 1);

            var command = new CreateAccountPaymentCommand(account.Id, new CreatePaymentCommand { Amount= amount, Date = date });

            var handler = new CreateAccountPaymentHandler(accountsRepo.Object);
            var paymentId = await handler.Handle(command, new CancellationToken());

            Assert.Equal(90, account.Balance);
            Assert.Equal(amount, account.PaymentRequests.First().Amount);
            Assert.Equal(date, account.PaymentRequests.First().Date);
            Assert.Equal(PaymentStatus.Pending, account.PaymentRequests.First().Status);
        }

        [Fact]
        public async void Handle_ShouldCreateClosedPayment()
        {
            var accountsRepo = new Mock<IAccountRepository>();

            var account = new Account(Guid.NewGuid(), 100);

            accountsRepo.Setup(a => a.GetAsync(account.Id)).ReturnsAsync(account);
            accountsRepo.Setup(p => p.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var amount = 200;
            var date = new DateTime(2020, 3, 1);

            var command = new CreateAccountPaymentCommand(account.Id, new CreatePaymentCommand { Amount = amount, Date = date });

            var handler = new CreateAccountPaymentHandler(accountsRepo.Object);
            var paymentId = await handler.Handle(command, new CancellationToken());

            Assert.Equal(100, account.Balance);
            Assert.Equal(amount, account.PaymentRequests.First().Amount);
            Assert.Equal(date, account.PaymentRequests.First().Date);
            Assert.Equal(PaymentStatus.Closed, account.PaymentRequests.First().Status);
        }
    }
}
