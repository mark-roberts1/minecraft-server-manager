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

        public async Task AddAsync(ServerInfo server, Template template, CancellationToken cancellationToken)
        {
            var wrapper = new ServerWrapper(server, _configuration, _diskOperator, _loggerFactory);

            await wrapper.DownloadTemplateAsync(template.DownloadLink, cancellationToken);

            _servers.Add(wrapper);
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

            wrapper.DeleteFolder();

            _servers.Remove(wrapper);

            response.ServerDeleted = true;

            return response;
        }

        public async Task<ServerCommandResponse> ExecuteCommandAsync(int serverId, ServerCommandRequest serverCommandRequest, CancellationToken cancellationToken)
        {
            if (serverCommandRequest.Command == "stop") throw new InvalidOperationException("Please use the Stop() method to stop the server.");

            var server = _servers.First(w => w.Server.ServerId == serverId);

            return await server.IssueCommandAsync(serverCommandRequest.Command, cancellationToken);
        }

        public Task<ServerInfo> GetAsync(int serverId, CancellationToken cancellationToken)
        {
            return Task.FromResult(Servers.FirstOrDefault(w => w.ServerId == serverId));
        }

        public Task<IEnumerable<ServerInfo>> ListAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Servers);
        }

        public Task<StartResponse> StartAsync(int serverId, CancellationToken cancellationToken)
        {
            var server = _servers.First(w => w.Server.ServerId == serverId);

            return Task.FromResult(server.Start());
        }

        public Task<bool> StopAsync(int serverId, CancellationToken cancellationToken)
        {
            var server = _servers.First(w => w.Server.ServerId == serverId);

            return server.StopAsync(cancellationToken);
        }

        public async Task UpdateAsync(int serverId, UpdateServerRequest updateServerRequest, Template template, CancellationToken cancellationToken)
        {
            var server = _servers.First(w => w.Server.ServerId == serverId);

            server.Server.Description = updateServerRequest.Description;
            server.Server.Name = updateServerRequest.NewName;

            server.UpdateProperties(updateServerRequest.NewProperties);

            if (server.Server.Version != updateServerRequest.Version)
            {
                await server.DownloadTemplateAsync(template.ThrowIfNull("template").DownloadLink, cancellationToken);
            }

            server.Server.Version = updateServerRequest.Version;
        }

        public ServerPropertyList GetServerProperties(int serverId)
        {
            return _servers.First(w => w.Server.ServerId == serverId).GetPropertiesFile();
        }
    }
}
