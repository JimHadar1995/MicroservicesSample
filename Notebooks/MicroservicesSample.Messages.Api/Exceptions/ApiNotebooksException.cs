using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroservicesSample.Common.Exceptions;

namespace MicroservicesSample.Messages.Api.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiNotebooksException : BaseException
    {
        /// <inheritdoc />
        public ApiNotebooksException()
        {
        }

        /// <inheritdoc />
        public ApiNotebooksException(string? message) : base(message)
        {
        }

        /// <inheritdoc />
        public ApiNotebooksException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
