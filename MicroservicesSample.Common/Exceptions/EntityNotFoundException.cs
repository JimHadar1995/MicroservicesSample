using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesSample.Common.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityNotFoundException : BaseException
    {
        /// <inheritdoc/>
        public EntityNotFoundException()
        {
        }

        /// <inheritdoc/>
        public EntityNotFoundException(string? message) : base(message)
        {
        }

        /// <inheritdoc/>
        public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
