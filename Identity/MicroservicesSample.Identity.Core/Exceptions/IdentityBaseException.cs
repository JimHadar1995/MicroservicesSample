using System;
using System.Collections.Generic;
using System.Text;
using MicroservicesSample.Common.Exceptions;

namespace MicroservicesSample.Identity.Core.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class IdentityBaseException : BaseException
    {
        /// <inheritdoc/>
        public IdentityBaseException()
        {
        }
        /// <inheritdoc/>
        public IdentityBaseException(string message) : base(message)
        {
        }
        /// <inheritdoc/>
        public IdentityBaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
