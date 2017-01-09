using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsPlanIcdEditApi.Filters
{
    /// <summary>
    /// This class can be used as a concrete implementation of the abstract LoggableClass.
    /// A class that already inherits from a base class can then use this instance class to perform logging.
    /// </summary>
    public class LoggableClassInstance : LoggableClass
    {
        public LoggableClassInstance(string loggerName, string className)
            : base(loggerName, className)
        {
        }
    }
}