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
        // TODO: import 
        // TODO: start 
        // TODO: stop 
        // TODO: command 
    }
}