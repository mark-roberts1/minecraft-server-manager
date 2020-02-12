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
    public class ServerWrapper
    {
        private readonly IDiskOperator _diskOperator;
        private readonly ILogger _logger;
        private readonly string _serverPath;
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

            if (!_diskOperator.FileExists(_propertiesPath))
            {
                using (_diskOperator.CreateFile(_propertiesPath)) { }

                _diskOperator.WriteAllLines(_propertiesPath, Server.Properties.GetLines().ToArray());
            }    
        }

        public ServerInfo Server { get; }

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

            Server.Status = ServerStatus.Started;

            return resp;
        }

        public async Task<bool> StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var rconClient = new RconClient("marksgamedomain.net", Server.Properties.RconPort))
                {
                    rconClient.LogAction = msg => _logger.Log(LogLevel.Info, $"From RconClient: \"{msg}\"");

                    await rconClient.ExecuteCommandAsync(RconCommand.Auth(Server.Properties.RconPassword), cancellationToken);

                    var response = await rconClient.ExecuteCommandAsync(RconCommand.ServerCommand("stop"), cancellationToken);
                    
                    _logger.Log(LogLevel.Info, response.ResponseText);
                }

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
            try
            {
                using (var rconClient = new RconClient("marksgamedomain.net", Server.Properties.RconPort))
                {
                    rconClient.LogAction = msg => _logger.Log(LogLevel.Info, $"From RconClient: \"{msg}\"");

                    rconClient.ExecuteCommand(RconCommand.Auth(Server.Properties.RconPassword));

                    var response = rconClient.ExecuteCommand(RconCommand.ServerCommand("stop"));

                    _logger.Log(LogLevel.Info, response.ResponseText);
                }

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
            try
            {
                using (var rconClient = new RconClient("marksgamedomain.net", Server.Properties.RconPort))
                {
                    rconClient.LogAction = msg => _logger.Log(LogLevel.Info, $"From RconClient: \"{msg}\"");

                    await rconClient.ExecuteCommandAsync(RconCommand.Auth(Server.Properties.RconPassword), cancellationToken);

                    var response = await rconClient.ExecuteCommandAsync(RconCommand.ServerCommand(command), cancellationToken);

                    _logger.Log(LogLevel.Info, response.ResponseText);

                    return new ServerCommandResponse
                    {
                        Log = response.ResponseText,
                        Succeeded = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);

                return new ServerCommandResponse
                {
                    Log = ex.Message,
                    Succeeded = false
                };
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
