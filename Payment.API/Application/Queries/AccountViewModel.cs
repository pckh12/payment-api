using Payment.API.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace Payment.API.Application.Queries
{
    public class AccountViewModel
    {
        public double Balance { get; set; }

        public List<PaymentViewModel> Payments { get; set; }

        public static AccountViewModel FromAccount(Account account)
        {
            return new AccountViewModel
            {
                Balance = account.Balance,
                Payments = account.PaymentRequests.OrderByDescending(a => a.Date).Select(p =>
                    new PaymentViewModel
                    {
                        Date = p.Date,
                        Amount = p.Amount,
                        Status = p.Status.ToString(),
                        Reason = p.Reason,
                        HRef = "/payments/" + p.Id.ToString()
                    }).ToList()
            };
        }
    }
}
