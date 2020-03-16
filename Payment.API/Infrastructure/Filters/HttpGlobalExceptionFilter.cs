using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Payment.API.Domain;
using System.Net;

namespace Payment.API.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(PaymentDomainException))
            {
                var details = new
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = context.Exception.Message.ToString()
                };

                context.Result = new BadRequestObjectResult(details);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                var details = new 
                {
                    Messages = new[] { "An unexpected error occured" }
                };

                var result = new ObjectResult(details);
                result.StatusCode = StatusCodes.Status500InternalServerError;

                context.Result = result;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }
    }
}
