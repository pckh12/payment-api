using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.API.Application.Commands
{
    public class CreatePaymentCommand
    {
        [Required]
        public double Amount { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }
    }
}
