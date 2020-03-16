using Payment.API.Domain;
using Payment.API.Domain.Model;
using System;
using System.Linq;
using Xunit;

namespace Payment.API.Test.UnitTests.Domain
{
    public class PaymentRequestTests
    {
        [Fact]
        public void Contructor_ShouldInitialise()
        {
            var date = new DateTime(2020, 3, 1);
            var amount = 10;
            var status = PaymentStatus.Pending;

            var model = new PaymentRequest(date, amount, status);

            Assert.Equal(date, model.Date);
            Assert.Equal(amount, model.Amount);
            Assert.Equal(status, model.Status);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100.23)]
        public void PaymentRequest_ShouldNotAcceptInvalidAmount(double amount)
        {
            var date = new DateTime(2020, 3, 1);
            var status = PaymentStatus.Pending;

            Assert.Throws<PaymentDomainException>(() => new PaymentRequest(date, amount, status));
        }

        [Fact]
        public void PaymentRequest_NewInsufficientFundsRequest()
        {
            var date = new DateTime(2020, 3, 1);
            var amount = 10;

            var model = PaymentRequest.NewInsufficientFundsPaymentRequest(date, amount);

            Assert.Equal(date, model.Date);
            Assert.Equal(amount, model.Amount);
            Assert.Equal(PaymentStatus.Closed, model.Status);
            Assert.Equal("Not enough funds", model.Reason);
        }

        [Fact]
        public void PaymentRequest_NewPendingPaymentRequest()
        {
            var date = new DateTime(2020, 3, 1);
            var amount = 10;

            var model = PaymentRequest.NewPendingPaymentRequest(date, amount);

            Assert.Equal(date, model.Date);
            Assert.Equal(amount, model.Amount);
            Assert.Equal(PaymentStatus.Pending, model.Status);
            Assert.True(string.IsNullOrEmpty(model.Reason));
        }

        public class ValidStatusChanges
        {
            PaymentRequest paymentRequest;

            public ValidStatusChanges()
            {
                var date = new DateTime(2020, 3, 1);
                var amount = 10;

                paymentRequest = PaymentRequest.NewPendingPaymentRequest(date, amount);
            }

            public void Dispose()
            {
                paymentRequest = null;
            }

            [Fact]
            public void CancelPayment_ShouldBeClosed()
            {
                var reason = "cancelled";
                paymentRequest.CancelPayment(reason);

                Assert.Equal(PaymentStatus.Closed, paymentRequest.Status);
                Assert.Equal(reason, paymentRequest.Reason);
            }

            [Fact]
            public void ProcessPayment_ShouldBeProcessed()
            {
                paymentRequest.ProcessPayment();

                Assert.Equal(PaymentStatus.Processed, paymentRequest.Status);
                Assert.Equal("Processed", paymentRequest.Reason);
            }

        }

        public class InvalidStatusChanges
        {
            PaymentRequest paymentRequest;

            public InvalidStatusChanges()
            {
                var date = new DateTime(2020, 3, 1);
                var amount = 10;

                paymentRequest = new PaymentRequest(date, amount, PaymentStatus.Closed);
            }

            public void Dispose()
            {
                paymentRequest = null;
            }

            [Fact]
            public void CancelPayment_ShouldThrow()
            {
                Assert.Throws<PaymentDomainException>(() => paymentRequest.CancelPayment("cancelled"));
            }

            [Fact]
            public void ProcessPayment_ShouldThrow()
            {
                Assert.Throws<PaymentDomainException>(() => paymentRequest.ProcessPayment());
            }

        }
    }
}
