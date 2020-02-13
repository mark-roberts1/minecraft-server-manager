using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerManager.Rest.Data;
using ServerManager.Rest.Dto;
using ServerManager.Rest.Filters;
using ServerManager.Rest.Management;

namespace ServerManager.Rest.Controllers
{
    [Route("api/server")]
    [EnableCors("AllowAny")]
    [ApiExceptionFilter]
    [ApiController]
    public class ServerController : ApiController
    {
        private readonly IServerData _serverData;
        private readonly IServerManager _serverManager;

        public ServerController(IUserData userData, IServerData serverData, IServerManager serverManager)
            : base(userData)
        {
            _serverManager = serverManager.ThrowIfNull(nameof(serverManager));
            _serverData = serverData.ThrowIfNull(nameof(serverData));

            if (!_serverManager.IsInitialized)
            {
                var serversTask = _serverData.ListAsync(default);
                serversTask.Wait();
                _serverManager.Initialize(serversTask.Result);
            }
        }

        [HttpGet("list")]
        public async Task<IEnumerable<ServerInfo>> ListAsync(CancellationToken cancellationToken)
        {
            ThrowIfUnauthenticated();

            return await _serverData.ListAsync(cancellationToken);
        }

        [HttpGet("{serverId}")]
        public async Task<ServerInfo> GetAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            ThrowIfUnauthenticated();

            return await _serverData.GetAsync(serverId, cancellationToken);
        }

        [HttpPost("create")]
        public async Task<CreateServerResponse> CreateAsync([FromBody] CreateServerRequest createRequest, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            var template = await _serverData.GetTemplateAsync(createRequest.Version, cancellationToken);

            if (template == null) throw new ArgumentException("createRequest.Version");

            var response = await _serverData.CreateAsync(createRequest, cancellationToken);

            var server = await _serverData.GetAsync(response.ServerId, cancellationToken);

            try
            {
                await _serverManager.AddAsync(server, template, cancellationToken);
            }
            catch
            {
                await _serverData.DeleteAsync(server.ServerId, cancellationToken);
                response.Created = false;
            }

            return response;
        }

        [HttpDelete("{serverId}/delete")]
        public async Task<DeleteServerResponse> DeleteAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            var deletedPhysical = await _serverManager.DeleteAsync(serverId, cancellationToken);

            var response = new DeleteServerResponse
            {
                ServerDeleted = deletedPhysical.ServerDeleted && (await _serverData.DeleteAsync(serverId, cancellationToken)).ServerDeleted
            };

            return response;
        }

        [HttpPut("{serverId}/update")]
        public async Task<UpdateServerResponse> UpdateAsync([FromRoute] int serverId, [FromBody] UpdateServerRequest updateRequest, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            var server = await _serverData.GetAsync(serverId, cancellationToken);

            Template template = null;

            if (server.Version != updateRequest.Version)
            {
                template = await _serverData.GetTemplateAsync(updateRequest.Version, cancellationToken);
            }

            await _serverManager.UpdateAsync(serverId, updateRequest, template, cancellationToken);

            return await _serverData.UpdateAsync(serverId, updateRequest, cancellationToken);
        }

        [HttpGet("templates")]
        public async Task<IEnumerable<Template>> ListTemplatesAsync(CancellationToken cancellationToken)
        {
            ThrowIfUnauthenticated();

            return await _serverData.ListTemplatesAsync(cancellationToken);
        }

        [HttpPost("template/add")]
        public async Task<AddTemplateResponse> AddTemplateAsync([FromBody] AddTemplateRequest addTemplateRequest, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            return await _serverData.AddTemplateAsync(addTemplateRequest, cancellationToken);
        }

        [HttpGet("template/{templateId}")]
        public async Task<Template> GetTemplateAsync([FromRoute] int templateId, CancellationToken cancellationToken)
        {
            ThrowIfUnauthenticated();

            return await _serverData.GetTemplateAsync(templateId, cancellationToken);
        }

        [HttpPut("template/{templateId}/update")]
        public async Task<UpdateTemplateResponse> UpdateTemplateAsync([FromBody] UpdateTemplateRequest updateTemplateRequest, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            return await _serverData.UpdateTemplateAsync(updateTemplateRequest, cancellationToken);
        }

        [HttpPost("{serverId}/start")]
        public async Task<StartResponse> StartAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();
            
            var didStart = await _serverManager.StartAsync(serverId, cancellationToken);

            var response = new StartResponse
            {
                DidStart = didStart.DidStart && (await _serverData.StartAsync(serverId, cancellationToken)).DidStart,
                Log = didStart.Log
            };

            return response;
        }

        [HttpPost("{serverId}/stop")]
        public async Task<bool> StopAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            var didStop = await _serverManager.StopAsync(serverId, cancellationToken);
            
            return didStop && await _serverData.StopAsync(serverId, cancellationToken);
        }

        [HttpPost("{serverId}/executecommand")]
        public async Task<ServerCommandResponse> ExecuteCommandAsync([FromRoute] int serverId, [FromBody] ServerCommandRequest serverCommandRequest, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            return await _serverManager.ExecuteCommandAsync(serverId, serverCommandRequest, cancellationToken);
        }

        [HttpGet("defaultproperties")]
        public async Task<ServerPropertyList> GetDefaultPropertiesAsync(CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            return await _serverData.GetDefaultPropertiesAsync(cancellationToken);
        }
    }
}