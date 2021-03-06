﻿using Moq;
using Payment.API.Application.Commands;
using Payment.API.Domain;
using Payment.API.Domain.Model;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace Payment.API.Test.UnitTests.Application
{
    public class ProcessPaymentHandlerTests
    {
        [Fact]
        public async void Handle_ShouldProcessPayment()
        {
            var accountsRepo = new Mock<IAccountRepository>();

            var account = new Account(Guid.NewGuid(), 100);
            account.AddPaymentRequest(new DateTime(2020, 3, 1), 40);

            accountsRepo.Setup(a => a.GetAsync(account.Id)).ReturnsAsync(account);

            var paymentRepo = new Mock<IPaymentRepository>();

            var payment = account.PaymentRequests.First();

            paymentRepo.Setup(p => p.GetAsync(payment.Id)).ReturnsAsync(payment);
            paymentRepo.Setup(p => p.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var reason = "Processed";
            var command = new ProcessPaymentCommand(account.Id, payment.Id);

            var handler = new ProcessPaymentHandler(accountsRepo.Object, paymentRepo.Object);
            await handler.Handle(command, new CancellationToken());

            Assert.Equal(PaymentStatus.Processed, payment.Status);
            Assert.Equal(reason, payment.Reason);
        }
    }
}
