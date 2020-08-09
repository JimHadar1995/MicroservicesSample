using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroservicesSample.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroservicesSample.Messages.Api.Internal
{
    /// <inheritdoc />
    internal class HttpGlobalExceptionFilter : IExceptionFilter
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
                default:
                    {
                        context.Result = new BadRequestObjectResult(context.Exception.Message);
                        break;
                    }
            }
        }
    }
}
