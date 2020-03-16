using MediatR;
using System;

namespace Payment.API.Application.Queries
{
    public class GetPaymentsQuery : IRequest<AccountViewModel>
    {
        public Guid AccountId { get; }
        public GetPaymentsQuery(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
