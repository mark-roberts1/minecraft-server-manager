using ServerManager.Rest.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Management
{
    public interface IServer
    {
        void RefreshSettings();
        ServerInfo Server { get; }
        StartResponse Start();
        Task<bool> StopAsync(CancellationToken cancellationToken);
        bool Stop();
        Task<ServerCommandResponse> IssueCommandAsync(string command, CancellationToken cancellationToken);
        void UpdateProperties(ServerPropertyList properties);
        void DeleteFolder();
        Task DownloadTemplateAsync(string link, CancellationToken cancellationToken);
        string[] GetPropertiesFile();
    }
}
