using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesSample.Common.Exceptions
{
    public class ConsulException : BaseException
    {
        public ConsulException()
        {
        }

        public ConsulException(string? message) : base(message)
        {
        }

        public ConsulException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
