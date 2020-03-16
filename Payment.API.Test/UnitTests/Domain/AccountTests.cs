using Payment.API.Application.Queries;
using Payment.API.Domain.Model;
using System;
using System.Linq;
using Xunit;

namespace Payment.API.Test.UnitTests.Domain
{
    public class AccountViewModelTests
    {
        [Fact]
        public void FromAccount_ShouldConvertToViewModel()
        {
            var id = Guid.NewGuid();
            var balance = 123.45;
            var account = new Account(id, balance);

            var date = new DateTime(2020, 3, 1);
            var amount = 10;
            account.AddPaymentRequest(date, amount);

            var viewModel = AccountViewModel.FromAccount(account);

            Assert.Equal(account.Balance, viewModel.Balance);
            Assert.Equal(account.PaymentRequests.First().Amount, viewModel.Payments.First().Amount);
            Assert.Equal(account.PaymentRequests.First().Date, viewModel.Payments.First().Date);
            Assert.Equal(account.PaymentRequests.First().Reason, viewModel.Payments.First().Reason);
            Assert.Equal(account.PaymentRequests.First().Status.ToString(), viewModel.Payments.First().Status);
        }

    }
}
