using Grpc.Core;
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
            switch (context.Exception)
            {
                case RpcException rpcEx:
                {
                    context.Result = new BadRequestObjectResult(rpcEx.Status.Detail);
                    break;
                }
                default:
                    {
                        context.Result = new BadRequestObjectResult("An unknown server error has occurred. Please contact technical support");
                        break;
                    }
            }
        }
    }
}
