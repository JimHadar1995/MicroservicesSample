using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesSample.Identity.Core.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при неуспешной авторизации
    /// </summary>
    public sealed class AuthorizationException : IdentityBaseException
    {
        /// <inheritdoc />
        public AuthorizationException()
        {
        }

        /// <inheritdoc />
        public AuthorizationException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public AuthorizationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
