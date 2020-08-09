using MicroservicesSample.ApiGateway.Exceptions;
using MicroservicesSample.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroservicesSample.ApiGateway.Code
{
    /// <inheritdoc />
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        /// <inheritdoc />
        public void OnException(ExceptionContext context)
        {
            if (context == null)
                return;
            switch (context.Exception)
            {
                case EntityNotFoundException _:
                    context.Result = new NotFoundResult();
                    break;
                case ErrorResponseException errorResponseEx:
                    context.Result = new BadRequestObjectResult(errorResponseEx.Message);
                    break;
                case ForbidException _:
                    context.Result = new ForbidResult();
                    break;
                case UnAuthorizedException _:
                    context.Result = new UnauthorizedResult();
                    break;
                default:
                    {
                        context.Result = new BadRequestObjectResult(context.Exception.Message);
                        break;
                    }
            }
        }
    }
}
