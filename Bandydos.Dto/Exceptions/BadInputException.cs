using System;

namespace Bandydos.Dto.Exceptions
{
    public class BadInputException : Exception
    {
        public BadInputException(string message) : base(message)
        {
        }
    }
}

