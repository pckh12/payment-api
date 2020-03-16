using System;
using System.Collections.Generic;

namespace Payment.API.Domain.Model
{
    /// <summary>
    /// Represents an account belonging to a user
    /// </summary>
    public class Account
    {
        public Guid Id { get; }
        public double Balance { get; private set; }

        private readonly List<PaymentRequest> _paymentRequests;
        public IReadOnlyCollection<PaymentRequest> PaymentRequests => _paymentRequests;

        public Account(Guid id, double balance)
        {
            Id = id;
            Balance = balance;
            _paymentRequests = new List<PaymentRequest>();
        }

        public Guid AddPaymentRequest(DateTime date, double amount)
        {
            // check whether the account has sufficient balance
            PaymentRequest paymentRequest;
            if (Balance < amount)
            {
                paymentRequest = PaymentRequest.NewInsufficientFundsPaymentRequest(date, amount);
            }
            else
            {
                paymentRequest = PaymentRequest.NewPendingPaymentRequest(date, amount);
                // reduce balance
                Balance -= amount;
            }

            _paymentRequests.Add(paymentRequest);

            return paymentRequest.Id;
        }

    }
}
