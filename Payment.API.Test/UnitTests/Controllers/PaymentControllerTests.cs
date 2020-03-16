using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Payment.API.Application.Commands;
using Payment.API.Application.Queries;
using Payment.API.Controllers;
using Payment.API.Domain;
using Payment.API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Payment.API.Test.UnitTests.Controllers
{
    public class PaymentControllerTests
    {
        [Fact]
        public async void GetPaymentsAsync_ShouldReturnViewModel()
        {
            var account = new AccountViewModel { Balance = 100 };

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<GetPaymentsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(account);


            var controller = new PaymentController(mediator.Object);

            var result = await controller.GetPaymentsAsync(Guid.NewGuid());

            Assert.Equal(account, result.Value);
        }

        [Fact]
        public async void CreatePaymentAsync_ShouldHaveIdInCreatedAt()
        { 
            var accountId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<CreateAccountPaymentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(paymentId);
            
            var controller = new PaymentController(mediator.Object);

            var result = (CreatedAtActionResult)await controller.CreatePaymentAsync(accountId, new CreatePaymentCommand { Amount = 10, Date = new DateTime(2020, 3, 1) });

            var id = Guid.Parse(result.RouteValues["id"].ToString());
            Assert.Equal(paymentId, id);
        }

        [Fact]
        public async void CancelPaymentAsync_ShouldReturnOk()
        {
            var accountId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<CancelPaymentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var controller = new PaymentController(mediator.Object);

            var result = await controller.CancelPaymentAsync(accountId, paymentId, new CancelReasonDTO { Reason = "cancel" });
            Assert.True(result is OkResult);
        }


        [Fact]
        public async void CancelPaymentAsync_ShouldReturnNotFound()
        {
            var accountId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<CancelPaymentCommand>(), It.IsAny<CancellationToken>())).Throws(new KeyNotFoundException());

            var controller = new PaymentController(mediator.Object);

            var result = await controller.CancelPaymentAsync(accountId, paymentId, new CancelReasonDTO { Reason = "cancel" });
            Assert.True(result is NotFoundResult);
        }

        [Fact]
        public async void ProcessPaymentAsync_ShouldReturnOk()
        {
            var accountId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<ProcessPaymentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var controller = new PaymentController(mediator.Object);

            var result = await controller.ProcessPaymentAsync(accountId, paymentId);
            Assert.True(result is OkResult);
        }


        [Fact]
        public async void ProcessPaymentAsync_ShouldReturnNotFound()
        {
            var accountId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<ProcessPaymentCommand>(), It.IsAny<CancellationToken>())).Throws(new KeyNotFoundException());

            var controller = new PaymentController(mediator.Object);

            var result = await controller.ProcessPaymentAsync(accountId, paymentId);
            Assert.True(result is NotFoundResult);
        }
    }
}
