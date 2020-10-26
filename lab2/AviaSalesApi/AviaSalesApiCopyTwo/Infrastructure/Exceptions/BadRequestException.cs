using System;

namespace AviaSalesApiCopyTwo.Infrastructure.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg) : base(msg)
        {
        }
    }
}