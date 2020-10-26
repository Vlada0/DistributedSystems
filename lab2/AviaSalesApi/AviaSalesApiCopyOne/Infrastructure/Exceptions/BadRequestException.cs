using System;

namespace AviaSalesApiCopyOne.Infrastructure.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg) : base(msg)
        {
        }
    }
}