using Microsoft.Extensions.Configuration;
using ServerManager.Rest.Dto;
using ServerManager.Rest.IO;
using ServerManager.Rest.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Management
{
    public class ServerManager : IServerManager
    {
        private static bool isInitialized;
        private static readonly List<ServerWrapper> _servers = new List<ServerWrapper>();
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDiskOperator _diskOperator;
        private readonly ILogger _logger;

        public IEnumerable<ServerInfo> Servers => _servers.Select(s => s.Server);
        public bool IsInitialized => isInitialized;

        public ServerManager(IConfiguration configuration, ILoggerFactory loggerFactory, IDiskOperator diskOperator)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
            _diskOperator = diskOperator;
            _logger = loggerFactory.GetLogger<ServerManager>();
        }

        public void Initialize(IEnumerable<ServerInfo> servers)
        {
            _logger.Log(LogLevel.Info, "Initializing the server manager...");

            foreach (var server in servers)
            {
                try
                {
                    _servers.Add(new ServerWrapper(server, _configuration, _diskOperator, _loggerFactory));
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Warning, "Failed to initialize server.");
                    _logger.Log(LogLevel.Warning, $"Server ID: {server.ServerId}, Server Name: {server.Name}");
                    _logger.Log(LogLevel.Warning, ex);
                }
            }

            _logger.Log(LogLevel.Info, "Server manager initialized.");

            isInitialized = true;
        }

        public void Add(ServerInfo server)
        {
            _servers.Add(new ServerWrapper(server, _configuration, _diskOperator, _loggerFactory));
        }

        public async Task<DeleteServerResponse> DeleteAsync(int serverId, CancellationToken cancellationToken)
        {
            var response = new DeleteServerResponse();

            var wrapper = _servers.FirstOrDefault(w => w.Server.ServerId == serverId);

            if (wrapper == null)
            {
                response.ServerDeleted = true;
                return response;
            }

            if (wrapper.Server.Status != ServerStatus.Stopped)
            {
                await wrapper.StopAsync(cancellationToken);
            }

            _servers.Remove(wrapper);

            wrapper.Dispose();

            response.ServerDeleted = true;

            return response;
        }

        public Task<ServerCommandResponse> ExecuteCommand(int serverId, ServerCommandRequest serverCommandRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ServerInfo> GetAsync(int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ServerInfo>> ListAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartAsync(int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StopAsync(int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateServerResponse> UpdateAsync(int serverId, UpdateServerRequest updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
