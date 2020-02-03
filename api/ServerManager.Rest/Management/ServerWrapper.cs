using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ServerManager.Rest.Logging;
using Rcon.Client;
using ServerManager.Rest.Dto;
using ServerManager.Rest.IO;
using System.Threading;

namespace ServerManager.Rest.Management
{
    public class ServerWrapper : IDisposable
    {
        private bool disposed;
        private IRconClient rconClient;
        private readonly ILogger _logger;
        private readonly string _serverPath;
        private bool authenticated;

        public ServerWrapper(ServerInfo serverInfo, IConfiguration configuration, IDiskOperator diskOperator, ILoggerFactory loggerFactory)
        {
            var serverAddress = configuration.GetValue<string>("AppSettings:ServerAddress");
            var serversBasePath = configuration.GetValue<string>("AppSettings:ServerDirectory");

            Server = serverInfo;

            _logger = loggerFactory.GetLogger<ServerWrapper>();
            _serverPath = diskOperator.CombinePaths(serversBasePath, serverInfo.GetUniqueServerName());

            RefreshSettings();
        }

        public void RefreshSettings()
        {
            if (Server.Properties.RconEnabled && rconClient == null)
                rconClient = new RconClient(_serverPath, Server.Properties.RconPort);
        }

        public ServerInfo Server { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public StartResponse Start()
        {
            var resp = new StartResponse();

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"java -Xms1G -Xmx1G -jar server.jar\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = _serverPath
                }
            };

            resp.DidStart = process.Start();
            resp.Log = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return resp;
        }

        public async Task<bool> StopAsync(CancellationToken cancellationToken)
        {
            if (rconClient == null) throw new InvalidOperationException("RCON is not enabled on this server.");

            LoginIfNecessary();

            try
            {
                var response = await rconClient.ExecuteCommandAsync(RconCommand.ServerCommand("stop"), cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return false;
            }
        }

        public bool Stop()
        {
            if (rconClient == null) throw new InvalidOperationException("RCON is not enabled on this server.");

            LoginIfNecessary();

            try
            {
                var response = rconClient.ExecuteCommand(RconCommand.ServerCommand("stop"));
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return false;
            }
        }

        public async Task<ServerCommandResponse> IssueCommand(string command)
        {
            if (rconClient == null) throw new InvalidOperationException("RCON is not enabled on this server.");

            LoginIfNecessary();

            try
            {
                var response = await rconClient.ExecuteCommandAsync(RconCommand.ServerCommand(command));

                return new ServerCommandResponse
                {
                    Succeeded = true,
                    Log = response.Text
                };
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);

                return new ServerCommandResponse
                {
                    Succeeded = false,
                    Log = ex.PrintFull()
                };
            }
        }

        private void LoginIfNecessary()
        {
            if (authenticated || !Server.Properties.RconEnabled || string.IsNullOrWhiteSpace(Server.Properties.RconPassword))
            {
                return;
            }

            try
            {
                var response = rconClient.ExecuteCommand(RconCommand.Login(Server.Properties.RconPassword));
                authenticated = true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                rconClient?.Dispose();
                disposed = true;
            }
        }
    }
}
