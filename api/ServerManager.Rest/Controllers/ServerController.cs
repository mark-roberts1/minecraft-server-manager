using System;
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

        /// <summary>
        /// Returns all available servers managed with this application
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet("list")]
        public async Task<IEnumerable<Server>> ListAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Define DTO ServerItem
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("{serverId}")]
        public async Task<Server> GetAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create the CreateServerRequest, and the CreateServerResponse
        /// </summary>
        /// <param name="createRequest"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("create")]
        public async Task<CreateServerResponse> CreateAsync([FromBody] CreateServerRequest createRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Define ServerDeleteResult
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="cancellationToken"></param>
        [HttpDelete("{serverId}/delete")]
        public async Task<DeleteServerResponse> DeleteAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Define UpdateServerRequest, UpdateServerResponse
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="updateRequest"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("{serverId}/update")]
        public async Task<UpdateServerResponse> UpdateAsync([FromRoute] int serverId, [FromBody] UpdateServerRequest updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // TODO: templates
        /// <summary>
        /// TODO: Define TDO Templates
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet("templates")]
        public async Task<IEnumerable<Template>> TemplateAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        // TODO: import
        /// <summary>
        /// TODO: Create ImportRequest, and ImportRequestResponse
        /// </summary>
        /// <param name="importRequest"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("import")]
        public async Task<IEnumerable<ImportServerResponse>> ImportAsync([FromBody] ImportServerRequest importRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        // TODO: start 
        /// <summary>
        /// TODO: Create executor for start command
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("{serverId}/start")]
        public async Task<bool> StartAsync([FromRoute] int serverId, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
        // TODO: stop 
        /// <summary>
        /// TODO: execute stop command in server console
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("{serverId}/stop")]
        public async Task<bool> StopAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        // TODO: command 
        /// <summary>
        /// TODO: create executor for command
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="serverCommandRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("{serverId}/executecommand")]
        public async Task<ServerCommandResponse> ExecuteCommand([FromRoute] int serverId, [FromBody] ServerCommandRequest serverCommandRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}