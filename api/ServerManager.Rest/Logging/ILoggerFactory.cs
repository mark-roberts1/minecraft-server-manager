using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManager.Rest.Logging
{
    public interface ILoggerFactory
    {
        ILogger GetLogger<T>();
    }
}
