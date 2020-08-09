using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesSample.Common.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityException : BaseException
    {
        /// <inheritdoc />
        public SecurityException()
        {
        }
        /// <inheritdoc />
        public SecurityException(string? message) : base(message)
        {
        }
        /// <inheritdoc />
        public SecurityException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
