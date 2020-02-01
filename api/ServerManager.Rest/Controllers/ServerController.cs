using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerManager.Rest.Controllers
{
    [Route("api/server")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        /// <summary>
        /// TODO: Define DTO ServerItem
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet("list")]
        public async Task<IEnumerable<object>> ListAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Define DTO ServerItem
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("{serverId}")]
        public async Task<object> GetAsync([FromRoute] int serverId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Create the CreateServerRequest, and the CreateServerResponse
        /// </summary>
        /// <param name="createServerRequest"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("create")]
        public async Task<object> CreateAsync([FromBody] object createRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: Define ServerDeleteResult
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="cancellationToken"></param>
        [HttpDelete("{serverId}/delete")]
        public async Task<object> DeleteAsync([FromRoute] int serverId, CancellationToken cancellationToken)
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
        public async Task<object> UpdateAsync([FromRoute] int serverId, [FromBody] object updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // TODO: templates
        /// <summary>
        /// TODO: Define TDO Templates
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet("templates")]
        public async Task<IEnumerable<object>> TemplatesAsync(CancellationToken cancellationToken)
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
        public async Task<IEnumerable<object>> ImportAsync([FromBody] object importRequest, CancellationToken cancellationToken)
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
        public async Task<IEnumerable<object>> StartAsync([FromRoute] int serverId, CancellationToken cancellationToken) {
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
        public async Task<IEnumerable<object>> StopAsync([FromRoute] int serverId, CancellationToken cancellationToken)
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
        public async Task<IEnumerable<object>> ExecuteCommand([FromRoute] int serverId, [FromBody] object serverCommandRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}