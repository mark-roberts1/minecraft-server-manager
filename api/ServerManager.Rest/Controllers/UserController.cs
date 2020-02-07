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
using ServerManager.Rest.Utility;

namespace ServerManager.Rest.Controllers
{
    [Route("api/user")]
    [EnableCors("AllowAny")]
    [ApiExceptionFilter]
    [ApiController]
    public class UserController : ApiController
    {
        private readonly ILinkGenerator _linkGenerator;

        public UserController(IUserData userData, ILinkGenerator linkGenerator)
            : base(userData)
        {
            _linkGenerator = linkGenerator.ThrowIfNull("linkGenerator");
        }

        [HttpGet]
        public async Task<User> GetSelfAsync(CancellationToken cancellationToken)
        {
            ThrowIfUnauthenticated();

            return await GetAuthenticatedUser();
        }

        [HttpGet("list")]
        public async Task<IEnumerable<User>> ListAsync(CancellationToken cancellationToken)
        {
            ThrowIfUnauthenticated();

            return await UserData.GetUsersAsync(cancellationToken);
        }

        [HttpGet("{userId}/get")]
        public async Task<User> GetUserAsync([FromRoute] int userId, CancellationToken cancellationToken)
        {
            await ThrowIfNotAdminOrNotAuthenticatedUser(userId);

            return await UserData.GetUserAsync(userId, cancellationToken);
        }

        [HttpPut("{userId}/updatePassword")]
        public async Task<UpdatePasswordResponse> UpdatePasswordAsync([FromRoute] int userId, [FromBody] UpdatePasswordRequest updateRequest, CancellationToken cancellationToken)
        {
            await ThrowIfNotAdminOrNotAuthenticatedUser(userId);

            return await UserData.UpdateUserPasswordAsync(userId, updateRequest, cancellationToken);
        }

        [HttpPut("{userId}/updateEmail")]
        public async Task<UpdateEmailResponse> UpdateEmailAsync([FromRoute] int userId, [FromBody] UpdateEmailRequest updateRequest, CancellationToken cancellationToken)
        {
            await ThrowIfNotAdminOrNotAuthenticatedUser(userId);

            return await UserData.UpdateUserEmailAsync(userId, updateRequest, cancellationToken);
        }

        [HttpPost("invite")]
        public async Task<InviteUserResponse> InviteUserAsync([FromBody] InviteUserRequest inviteRequest, CancellationToken cancellationToken)
        {
            ThrowIfUnauthenticated();

            var link = _linkGenerator.GenerateUniqueLink();


            var response = new InviteUserResponse
            {
                UserInvited = await _linkGenerator.SendInvitationLink(inviteRequest.EmailAddress, link, cancellationToken)
            };

            if (response.UserInvited)
            {
                await UserData.StoreInvitationLinkAsync(inviteRequest.EmailAddress, link, cancellationToken);
            }

            return response;
        }

        [HttpGet("{link}/validate")]
        public async Task<LinkValidationResponse> ValidateAsync([FromRoute] string link, CancellationToken cancellationToken)
        {
            return new LinkValidationResponse
            {
                IsValid = await UserData.IsLinkValidAsync(link, cancellationToken)
            };
        }

        [HttpPost("register")]
        public async Task<CreateUserResponse> CreateUserAsync([FromBody] CreateUserRequest createAccountRequest, CancellationToken cancellationToken)
        {
            if (!(await UserData.IsLinkValidAsync(createAccountRequest.InvitationLink, cancellationToken)))
                throw new UnauthorizedAccessException();

            return await UserData.CreateUserAsync(createAccountRequest, cancellationToken);
        }

        [HttpPut("{userId}/updateRole")]
        public async Task<UpdateRoleResponse> UpdateRoleAsync([FromRoute] int userId, [FromBody] UpdateRoleRequest updateRequest, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            return await UserData.UpdateUserRole(userId, updateRequest.UserRole, cancellationToken);
        }

        [HttpDelete("{userId}/delete")]
        public async Task<DeleteUserResponse> DeleteAsync([FromRoute] int userId, CancellationToken cancellationToken)
        {
            await ThrowIfNotAdminOrNotAuthenticatedUser(userId);

            return await UserData.DeleteUserAsync(userId, cancellationToken);
        }

        [HttpPost("{userId}/togglelock")]
        public async Task<ToggleUserLockResponse> ToggleUserLockAsync([FromRoute]int userId, CancellationToken cancellationToken)
        {
            ThrowIfNotAdmin();

            return await UserData.ToggleUserLockAsync(userId, cancellationToken);
        }
    }
}