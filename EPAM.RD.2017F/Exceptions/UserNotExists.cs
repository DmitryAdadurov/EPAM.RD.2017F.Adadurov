using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.RD._2017F.Exceptions
{
    public class UserNotExists : Exception
    {
        public UserNotExists() { }
        public UserNotExists(string message) : base(message) { }
        public UserNotExists(string message, Exception innerException) : base(message, innerException) { }
    }
}
