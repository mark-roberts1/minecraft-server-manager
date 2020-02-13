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
    public class MinecraftServerFactory : IServerFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IDiskOperator _diskOperator;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IServerStarter _serverStarter;
        private readonly IRconClientFactory _rconClientFactory;

        public MinecraftServerFactory(
            IConfiguration configuration, 
            IDiskOperator diskOperator, 
            ILoggerFactory loggerFactory, 
            IServerStarter serverStarter,
            IRconClientFactory rconClientFactory)
        {
            _configuration = configuration;
            _diskOperator = diskOperator;
            _loggerFactory = loggerFactory;
            _serverStarter = serverStarter;
            _rconClientFactory = rconClientFactory;
        }

        public IServer BuildServer(ServerInfo serverInfo)
        {
            return new MinecraftServer(serverInfo, _configuration, _diskOperator, _loggerFactory, _serverStarter, _rconClientFactory);
        }
    }
}
