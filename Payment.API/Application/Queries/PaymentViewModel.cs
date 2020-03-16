using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.API.Application.Queries
{
    public class PaymentViewModel
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string HRef { get; set; }
    }
}
