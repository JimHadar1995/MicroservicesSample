using System;
using MicroservicesSample.Common.Exceptions;

namespace MicroservicesSample.Identity.Core.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class RefreshTokenNotFoundException : EntityNotFoundException
    {
        /// <inheritdoc />
        public RefreshTokenNotFoundException()
        {
        }

        /// <inheritdoc />
        public RefreshTokenNotFoundException(string? message) : base(message)
        {
        }

        /// <inheritdoc />
        public RefreshTokenNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
