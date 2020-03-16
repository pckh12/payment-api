using Payment.API.Domain.Model;
using System;
using System.Linq;
using Xunit;

namespace Payment.API.Test.UnitTests.Application
{
    public class AccountViewModelTests
    {
        [Fact]
        public void Contructor_ShouldInitialise()
        {
            var id = Guid.NewGuid();
            var balance = 123.45;

            var model = new Account(id, balance);

            Assert.Equal(id, model.Id);
            Assert.Equal(balance, model.Balance);
            Assert.Equal(0, model.PaymentRequests.Count);
        }

        public class AddPaymentRequest
        {
            double originalBalance;
            Account account;

            public AddPaymentRequest()
            {
                var id = Guid.NewGuid();
                originalBalance = 100;

                account = new Account(id, originalBalance);
            }

            public void Dispose()
            {
                account = null;
            }

            [Fact]
            public void AddPaymentRequest_ShouldAddPendingRequest()
            {
                var date = new DateTime(2020, 3, 1);
                var amount = 10;

                var paymentId = account.AddPaymentRequest(date, amount);
                var paymentRequest = account.PaymentRequests.FirstOrDefault(p => p.Id == paymentId);

                Assert.Equal((originalBalance - amount), account.Balance);
                Assert.NotNull(paymentRequest);
                Assert.Equal(date, paymentRequest.Date);
                Assert.Equal(amount, paymentRequest.Amount);
                Assert.True(string.IsNullOrEmpty(paymentRequest.Reason));
                Assert.Equal(PaymentStatus.Pending, paymentRequest.Status);
            }

            [Fact]
            public void AddPaymentRequest_ShouldAddClosedRequest()
            {
                var date = new DateTime(2020, 3, 1);
                var amount = originalBalance + 10;

                var paymentId = account.AddPaymentRequest(date, amount);
                var paymentRequest = account.PaymentRequests.FirstOrDefault(p => p.Id == paymentId);

                Assert.Equal(originalBalance, account.Balance);
                Assert.NotNull(paymentRequest);
                Assert.Equal(date, paymentRequest.Date);
                Assert.Equal(amount, paymentRequest.Amount);
                Assert.Equal("Not enough funds", paymentRequest.Reason);
                Assert.Equal(PaymentStatus.Closed, paymentRequest.Status);
            }
        }
    }
}
