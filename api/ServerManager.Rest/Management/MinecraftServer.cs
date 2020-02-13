using System;
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
    public class MinecraftServer : IServer
    {
        private readonly IDiskOperator _diskOperator;
        private readonly ILogger _logger;
        private readonly IRconClientFactory _rconClientFactory;
        private readonly IServerStarter _serverStarter;
        private readonly OperatingSystem _targetOs;
        private readonly string _rconAddress;
        private readonly string _serverPath;
        private string _propertiesPath => _diskOperator.CombinePaths(_serverPath, "server.properties");

        public MinecraftServer(ServerInfo serverInfo, 
            IConfiguration configuration, 
            IDiskOperator diskOperator, 
            ILoggerFactory loggerFactory,
            IServerStarter serverStarter,
            IRconClientFactory rconClientFactory)
        {
            _rconAddress = configuration.GetValue<string>("AppSettings:RconAddress");
            var serversBasePath = configuration.GetValue<string>("AppSettings:ServerDirectory");
            _targetOs = configuration.GetValue<OperatingSystem>("AppSettings:OperatingSystem");

            Server = serverInfo;

            _logger = loggerFactory.GetLogger<MinecraftServer>();
            _serverPath = diskOperator.CombinePaths(serversBasePath, serverInfo.GetUniqueServerName());
            _diskOperator = diskOperator;
            _rconClientFactory = rconClientFactory.ThrowIfNull("rconClientFactory");
            _serverStarter = serverStarter.ThrowIfNull("serverStarter");

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

            resp.DidStart = _serverStarter.StartServer(_serverPath, 512, 1024, _targetOs);

            Server.Status = ServerStatus.Started;

            return resp;
        }

        public async Task<bool> StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var rconClient = _rconClientFactory.GetRconClient(_rconAddress, Server.Properties.RconPort))
                {
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
                using (var rconClient = _rconClientFactory.GetRconClient(_rconAddress, Server.Properties.RconPort))
                {
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
                using (var rconClient = _rconClientFactory.GetRconClient(_rconAddress, Server.Properties.RconPort))
                {
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
