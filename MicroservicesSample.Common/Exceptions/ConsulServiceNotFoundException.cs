using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesSample.Common.Exceptions
{
    public class ConsulServiceNotFoundException : ConsulException
    {
        public ConsulServiceNotFoundException()
        {
        }

        public ConsulServiceNotFoundException(string? message) : base(message)
        {
        }

        public ConsulServiceNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
