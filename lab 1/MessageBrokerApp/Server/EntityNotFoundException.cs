using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    internal class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string msg) : base(msg)
        {
        }
    }
}
