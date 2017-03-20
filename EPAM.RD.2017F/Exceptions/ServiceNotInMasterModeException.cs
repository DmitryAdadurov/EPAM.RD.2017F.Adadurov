using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.RD._2017F.Exceptions
{
    public class ServiceNotInMasterModeException : Exception
    {
        public ServiceNotInMasterModeException() { }
        public ServiceNotInMasterModeException(string message) : base(message) { }
        public ServiceNotInMasterModeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
