using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerManager.Rest.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //list
        /// <summary>
        /// TODO: Define DTO UserItem
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet("list")]
        public async Task<IEnumerable<object>> ListAsync(CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
        //get self
        /// <summary>
        /// TODO: Define DTO UserItem
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("{userId}/get")]
        public async Task<IEnumerable<object>> GetSelfAsync([FromRoute] int userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut("{userId}/updatePassword")]
        //change password
        /// <summary>
        /// TODO: define updatePassword, updateRequest
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateRequest"></param>
        /// <param name="cancellationToken"></param>
        public async Task<IEnumerable<object>> UpdatePasswordAsync([FromRoute] int userId, [FromBody] object updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        //change username
        /// <summary>
        /// TODO: define updateUsername, updateRequest
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{userId}/updateUsername")]
        public async Task<IEnumerable<object>> UpdateUsernameAsync([FromRoute] int userId, [FromBody] object updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        //invite user
        /// <summary>
        /// TODO: define inviteRequest
        /// </summary>
        /// <param name="inviteRequest"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("invite")]
        public async Task<IEnumerable<object>> InviteUser([FromBody] object inviteRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        //validate link
        /// <summary>
        /// TODO: Access data to check link
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("{userId}/validate")]
        
        public async Task<IEnumerable<object>> ValidateAsync([FromRoute] int userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        //create user(using link)
        /// <summary>
        /// TODO: define createAccountRequest
        /// TODO: create new User object and store information
        /// </summary>
        /// <param name="createAccountRequest"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("register")]
        public async Task<IEnumerable<object>> CreateUser([FromBody] object createAccountRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        //set role
        /// <summary>
        /// TODO: define updateRequest
        /// TODO: create RoleList and assign roles to list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{userId}/updateRole")]
        public async Task<IEnumerable<object>> UpdateRoleAsync([FromRoute] int userId, [FromBody] object updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        //delete
        /// <summary>
        /// TODO: define deleteUserRequest
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deleteUserRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{userId}/delete")]
        public async Task<IEnumerable<object>> Delete([FromRoute] int userId, [FromBody] object deleteUserRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}