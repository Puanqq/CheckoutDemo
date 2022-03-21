using System;
using System.Globalization;

namespace Checkout.API.Exceptions
{
    public class AppException : Exception
    {
        public AppException() : base() { }
        public AppException(string message) : base(message) { }        
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message = null)
            : base(message == null ? "Bad Request" : message)
        { }
    }

    public class ActionInputIsNotValidException : BadRequestException
    {
        public ActionInputIsNotValidException()
            : base("Action input is not valid")
        { }
    }
}
