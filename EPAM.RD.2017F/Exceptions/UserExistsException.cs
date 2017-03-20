using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.RD._2017F.Exceptions
{
    public class ExistingUserException : Exception
    {
        public ExistingUserException() { }
        public ExistingUserException(string message) : base(message) { }
        public ExistingUserException(string message, Exception innerException) : base(message, innerException) { }
    }
}
