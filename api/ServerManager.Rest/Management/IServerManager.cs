using ServerManager.Rest.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Management
{
    public interface IServerManager : IServerOperator
    {
        bool IsInitialized { get; }
        IEnumerable<ServerInfo> Servers { get; }
        void Initialize(IEnumerable<ServerInfo> servers);
        Task AddAsync(ServerInfo server, Template template, CancellationToken cancellationToken);
        Task UpdateAsync(int serverId, UpdateServerRequest updateServerRequest, Template template, CancellationToken cancellationToken);
        ServerPropertyList GetServerProperties(int serverId);
        Task<ServerCommandResponse> ExecuteCommandAsync(int serverId, ServerCommandRequest serverCommandRequest, CancellationToken cancellationToken);
    }
}
