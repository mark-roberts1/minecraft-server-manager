using ServerManager.Rest.Dto;
using ServerManager.Rest.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Data
{
    public class ServerData : IServerData
    {
        private readonly IServerManager _serverManager;

        public ServerData(IServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        public Task<AddTemplateResponse> AddTemplateAsync(AddTemplateRequest addTemplateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CreateServerResponse> CreateAsync(CreateServerRequest createRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteServerResponse> DeleteAsync(int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ServerCommandResponse> ExecuteCommand(int serverId, ServerCommandRequest serverCommandRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ServerInfo> GetAsync(int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Template> GetTemplateAsync(int templateId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ServerInfo>> ListAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Template>> ListTemplatesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<StartResponse> StartAsync(int serverId, CancellationToken cancellationToken)
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

        public Task<UpdateTemplateResponse> UpdateTemplateAsync(UpdateTemplateRequest updateTemplateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
