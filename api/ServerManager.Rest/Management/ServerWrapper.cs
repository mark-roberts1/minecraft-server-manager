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
using System.Net;

namespace ServerManager.Rest.Management
{
    public class ServerWrapper : IDisposable
    {
        private bool disposed;
        private IRconClient rconClient;
        private readonly IDiskOperator _diskOperator;
        private readonly ILogger _logger;
        private readonly string _serverPath;
        private bool authenticated;
        private string _propertiesPath => _diskOperator.CombinePaths(_serverPath, "server.properties");

        public ServerWrapper(ServerInfo serverInfo, IConfiguration configuration, IDiskOperator diskOperator, ILoggerFactory loggerFactory)
        {
            var serverAddress = configuration.GetValue<string>("AppSettings:ServerAddress");
            var serversBasePath = configuration.GetValue<string>("AppSettings:ServerDirectory");

            Server = serverInfo;

            _logger = loggerFactory.GetLogger<ServerWrapper>();
            _serverPath = diskOperator.CombinePaths(serversBasePath, serverInfo.GetUniqueServerName());
            _diskOperator = diskOperator;

            RefreshSettings();
        }

        public void RefreshSettings()
        {
            if (!_diskOperator.DirectoryExists(_serverPath))
                _diskOperator.CreateDirectory(_serverPath);

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
            if (!_diskOperator.FileExists(_diskOperator.CombinePaths(_serverPath, "eula.txt")))
            {
                using (_diskOperator.CreateFile(_diskOperator.CombinePaths(_serverPath, "eula.txt"))) { }

                _diskOperator.WriteAllLines(_diskOperator.CombinePaths(_serverPath, "eula.txt"), new string[] { "eula=true" });
            }

            var resp = new StartResponse();

            using var process = new Process
            {
                // TODO: Make this work for windows or linux
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

            Server.Status = ServerStatus.Started;

            return resp;
        }

        public async Task<bool> StopAsync(CancellationToken cancellationToken)
        {
            if (rconClient == null) throw new InvalidOperationException("RCON is not enabled on this server.");

            LoginIfNecessary();
            
            try
            {
                var response = await rconClient.ExecuteCommandAsync(RconCommand.ServerCommand("stop"), cancellationToken);
                Server.Status = ServerStatus.Stopped;
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
                Server.Status = ServerStatus.Stopped;
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return false;
            }
        }

        public async Task<ServerCommandResponse> IssueCommandAsync(string command, CancellationToken cancellationToken)
        {
            if (rconClient == null) throw new InvalidOperationException("RCON is not enabled on this server.");

            LoginIfNecessary();

            try
            {
                var response = await rconClient.ExecuteCommandAsync(RconCommand.ServerCommand(command), cancellationToken);

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

        public void UpdateProperties(ServerPropertyList properties)
        {
            if (properties.Equals(Server.Properties)) return;

            var wasRunning = Server.Status == ServerStatus.Started;

            if (wasRunning)
            {
                Stop();
            }

            if (_diskOperator.FileExists(_propertiesPath))
            {
                _diskOperator.DeleteFile(_propertiesPath);
            }

            using (var stream = _diskOperator.CreateFile(_propertiesPath))
            {
            }

            _diskOperator.WriteAllLines(_propertiesPath, properties.GetLines().ToArray());
            
            Server.Properties = properties;

            if (wasRunning)
            {
                Start();
            }
        }

        public void DeleteFolder()
        {
            _diskOperator.DeleteDirectory(_serverPath, true);
        }

        public async Task DownloadTemplateAsync(string link, CancellationToken cancellationToken)
        {
            var wasRunning = Server.Status == ServerStatus.Started;

            if (wasRunning)
            {
                Stop();
            }

            if (_diskOperator.FileExists(_diskOperator.CombinePaths(_serverPath, "server.jar")))
            {
                _diskOperator.DeleteFile(_diskOperator.CombinePaths(_serverPath, "server.jar"));
            }

            using (var client = new WebClient())
            {
                bool downloaded = false;

                client.DownloadFileCompleted += (object sender, System.ComponentModel.AsyncCompletedEventArgs e) =>
                {
                    downloaded = true;
                };

                client.DownloadFileAsync(new Uri(link), _diskOperator.CombinePaths(_serverPath, "server.jar"));

                try
                {
                    while (!downloaded)
                    {
                        await Task.Delay(1, cancellationToken);
                    }
                }
                catch (TaskCanceledException)
                {
                    client.CancelAsync();

                    throw;
                }
            }

            if (wasRunning)
            {
                Start();
            }
        }

        public string[] GetPropertiesFile()
        {
            if (!_diskOperator.FileExists(_propertiesPath))
            {
                return new string[] { };
            }

            return _diskOperator.ReadFileLines(_propertiesPath);
        }
    }
}
