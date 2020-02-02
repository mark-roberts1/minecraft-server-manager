﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerManager.Rest.Data;
using ServerManager.Rest.Dto;

namespace ServerManager.Rest.Controllers
{
    [Route("api/server")]
    [ApiController]
    public class ServerController : ApiController
    {
        public ServerController(IDataAccessLayer dataAccessLayer)
            : base(dataAccessLayer)
        {

        }

        [HttpGet("list")]
        public async Task<IEnumerable<Server>> ListAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{serverId}")]
        public async Task<Server> GetAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPost("create")]
        public async Task<CreateServerResponse> CreateAsync([FromBody] CreateServerRequest createRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{serverId}/delete")]
        public async Task<DeleteServerResponse> DeleteAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{serverId}/update")]
        public async Task<UpdateServerResponse> UpdateAsync([FromRoute] int serverId, [FromBody] UpdateServerRequest updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("templates")]
        public async Task<IEnumerable<Template>> ListTemplatesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPost("template/add")]
        public async Task<AddTemplateResponse> AddTemplateAsync(AddTemplateRequest addTemplateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("{serverId}/start")]
        public async Task<bool> StartAsync([FromRoute] int serverId, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
        
        [HttpPost("{serverId}/stop")]
        public async Task<bool> StopAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("{serverId}/executecommand")]
        public async Task<ServerCommandResponse> ExecuteCommand([FromRoute] int serverId, [FromBody] ServerCommandRequest serverCommandRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}