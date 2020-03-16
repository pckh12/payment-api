using System;

namespace Payment.API.Domain
{
    public class PaymentDomainException : Exception
    {
        public PaymentDomainException()
        { }

        public PaymentDomainException(string message)
            : base(message)
        { }

        public PaymentDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
