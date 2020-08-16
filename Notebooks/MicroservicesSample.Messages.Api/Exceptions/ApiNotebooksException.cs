using System;
using MicroservicesSample.Common.Exceptions;

namespace MicroservicesSample.Notebooks.Api.Exceptions
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
