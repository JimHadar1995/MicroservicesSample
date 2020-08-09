using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesSample.Identity.Core.Exceptions
{
    /// <summary>
    /// Исключение отзыва refresh
    /// </summary>
    public sealed class RevokedRefreshTokenException : IdentityBaseException
    {
        /// <inheritdoc />
        public RevokedRefreshTokenException()
        {
        }
    }
}
