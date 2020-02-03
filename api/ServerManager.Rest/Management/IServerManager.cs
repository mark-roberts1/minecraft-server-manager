using ServerManager.Rest.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Management
{
    public interface IServerManager : IServerOperator
    {
        bool IsInitialized { get; }
        IEnumerable<ServerInfo> Servers { get; }
        void Initialize(IEnumerable<ServerInfo> servers);
        void Add(ServerInfo server);
    }
}
