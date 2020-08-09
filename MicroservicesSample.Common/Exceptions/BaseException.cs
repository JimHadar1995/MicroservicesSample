using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MicroservicesSample.Common.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(string? message) : base(message)
        {
        }

        public BaseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

    }
}
