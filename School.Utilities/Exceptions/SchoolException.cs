using System;
using System.Collections.Generic;
using System.Text;

namespace School.Utilities.Exceptions
{
    public class SchoolException : Exception
    {
        public SchoolException()
        {
        }

        public SchoolException(string message)
            : base(message)
        {
        }

        public SchoolException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
