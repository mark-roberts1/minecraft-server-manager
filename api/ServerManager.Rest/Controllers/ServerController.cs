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

namespace ServerManager.Rest.Controllers
{
    [Route("api/server")]
    [EnableCors("AllowAny")]
    [ApiExceptionFilter]
    [ApiController]
    public class ServerController : ApiController
    {
        private readonly IServerData _serverData;

        public ServerController(IUserData userData, IServerData serverData)
            : base(userData)
        {
            _serverData = serverData.ThrowIfNull(nameof(serverData));
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

            return await _serverData.CreateAsync(createRequest, cancellationToken);
        }

        [HttpDelete("{serverId}/delete")]
        public async Task<DeleteServerResponse> DeleteAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            return await _serverData.DeleteAsync(serverId, cancellationToken);
        }

        [HttpPut("{serverId}/update")]
        public async Task<UpdateServerResponse> UpdateAsync([FromRoute] int serverId, [FromBody] UpdateServerRequest updateRequest, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

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

            return await _serverData.StartAsync(serverId, cancellationToken);
        }

        [HttpPost("{serverId}/stop")]
        public async Task<bool> StopAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            return await _serverData.StopAsync(serverId, cancellationToken);
        }

        [HttpPost("{serverId}/executecommand")]
        public async Task<ServerCommandResponse> ExecuteCommand([FromRoute] int serverId, [FromBody] ServerCommandRequest serverCommandRequest, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            return await _serverData.ExecuteCommandAsync(serverId, serverCommandRequest, cancellationToken);
        }
    }
}