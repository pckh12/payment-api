using System;

namespace Payment.API.Domain.Model
{
    /// <summary>
    /// Represents a payment request
    /// </summary>
    public class PaymentRequest
    {
        public Guid Id { get; }
        public DateTime Date { get; }
        public double Amount { get; }
        public PaymentStatus Status { get; private set; }
        public string Reason { get; private set; }

        public static PaymentRequest NewInsufficientFundsPaymentRequest(DateTime date, double amount)
        {
            var request = new PaymentRequest(date, amount, PaymentStatus.Closed);
            request.Reason = "Not enough funds";

            return request;
        }

        public static PaymentRequest NewPendingPaymentRequest(DateTime date, double amount)
        {
            var request = new PaymentRequest(date, amount, PaymentStatus.Pending);

            return request;
        }

        public PaymentRequest (DateTime date, double amount, PaymentStatus status)
        {
            if (amount <= 0)
            {
                throw new PaymentDomainException("Payment amount must be greater than zero");
            }

            Id = Guid.NewGuid();
            Date = date;
            Amount = amount;
            Status = status;
        }

        public void CancelPayment(string reason)
        {
            UpdateStatusFromPending(PaymentStatus.Closed, reason);
        }

        public void ProcessPayment()
        {
            UpdateStatusFromPending(PaymentStatus.Processed, "Processed");
        }

        private void UpdateStatusFromPending(PaymentStatus status, string reason)
        {
            if (Status != PaymentStatus.Pending)
            {
                throw new PaymentDomainException("Unable to change status. Current status must be Pending");
            }

            Status = status;
            Reason = reason;
        }
    }
}
