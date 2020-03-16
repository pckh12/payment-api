using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Application.Commands;
using Payment.API.Application.Queries;

namespace Payment.API.Controllers
{
    [Produces("application/json")]
    [Route("paymentapi/")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;


        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves details of the account and payment requests
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [Route("payments")]
        [HttpGet]
        [ProducesResponseType(typeof(AccountViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<AccountViewModel>> GetPaymentsAsync([FromHeader(Name = "ACCOUNT_KEY")] Guid accountId)
        {
            var query = new GetPaymentsQuery(accountId);
            var account = await _mediator.Send(query);

            return account;
        }

        /// <summary>
        /// Creates a payment request against the nominated account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="createPaymentCommand"></param>
        /// <returns></returns>
        [Route("payments")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreatePaymentAsync([FromHeader(Name = "ACCOUNT_KEY")] Guid accountId, [FromBody] CreatePaymentCommand createPaymentCommand)
        {
            var createAccountPaymentCommand = new CreateAccountPaymentCommand(accountId, createPaymentCommand);
            var paymentId = await _mediator.Send(createAccountPaymentCommand);

            return CreatedAtAction(nameof(GetPaymentsAsync), new { id = paymentId }, null);
        }

        /// <summary>
        /// Cancels a payment request
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="id"></param>
        /// <param name="cancelReason"></param>
        /// <returns></returns>
        [Route("payments/{id:guid}/cancel")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> CancelPaymentAsync([FromHeader(Name = "ACCOUNT_KEY")] Guid accountId, Guid id, [FromBody] CancelReasonDTO cancelReason)
        {
            var command = new CancelPaymentCommand(accountId, id, cancelReason.Reason);
            try
            {
                var response = await _mediator.Send(command);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Processes a payment request
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("payments/{id:guid}/process")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ProcessPaymentAsync([FromHeader(Name = "ACCOUNT_KEY")] Guid accountId, Guid id)
        {
            var command = new ProcessPaymentCommand(accountId, id);
            try
            {
                var response = await _mediator.Send(command);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

    }
}