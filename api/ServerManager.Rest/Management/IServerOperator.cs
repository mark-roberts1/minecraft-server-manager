﻿using ServerManager.Rest.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Management
{
    public interface IServerOperator
    {
        Task<IEnumerable<ServerInfo>> ListAsync(CancellationToken cancellationToken);
        Task<ServerInfo> GetAsync(int serverId, CancellationToken cancellationToken);
        Task<DeleteServerResponse> DeleteAsync(int serverId, CancellationToken cancellationToken);
        Task<StartResponse> StartAsync(int serverId, CancellationToken cancellationToken);
        Task<bool> StopAsync(int serverId, CancellationToken cancellationToken);
    }
}
