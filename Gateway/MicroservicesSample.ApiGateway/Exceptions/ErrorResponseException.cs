using System;
using MicroservicesSample.Common.Exceptions;

namespace MicroservicesSample.ApiGateway.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorResponseException : BaseException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ErrorResponseException(string? message) : base(message)
        {
        }
    }
}
