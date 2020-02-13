using Microsoft.Extensions.Configuration;
using ServerManager.Rest.Dto;
using ServerManager.Rest.IO;
using ServerManager.Rest.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Management
{
    public interface IServerFactory
    {
        public IServer BuildServer(ServerInfo serverInfo);
    }
}
