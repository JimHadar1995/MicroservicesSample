using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroservicesSample.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroservicesSample.Identity.Api.Code
{
    /// <inheritdoc />
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        /// <inheritdoc />
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case EntityNotFoundException _:
                    context.Result = new NotFoundResult();
                    break;
                default:
                {
                    string message = context.Exception.Message;
                    if (context.Exception.InnerException != null)
                    {
                        message += $"Reason: {context.Exception.InnerException.Message}";
                    }

                    context.Result = new BadRequestObjectResult(message);
                    break;
                }
            }
        }
    }
}
