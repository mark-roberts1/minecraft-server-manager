using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerManager.Rest.Data;
using ServerManager.Rest.Database;
using ServerManager.Rest.Dto;
using ServerManager.Rest.Filters;
using ServerManager.Rest.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Controllers
{
    [Route("api/auth")]
    [EnableCors("AllowAny")]
    [ApiExceptionFilter]
    [ApiController]
    public class LoginController : ApiController
    {
        private readonly ILinkGenerator _linkGenerator;
        
        public LoginController(IUserData userData, ILinkGenerator linkGenerator)
            : base(userData)
        {
            _linkGenerator = linkGenerator.ThrowIfNull("linkGenerator");
        }

        [HttpPost("token")]
        public async Task<TokenResponse> LoginAsync([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var result = await UserData.LoginAsync(loginRequest.ThrowIfNull("loginRequest"), cancellationToken);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException();
            }

            var token = _linkGenerator.GenerateUniqueLink();

            await UserData.StoreUserSessionTokenAsync(loginRequest.Username, token, cancellationToken);

            return new TokenResponse
            {
                Token = token
            };
        }

        [HttpPost("forgotpassword")]
        public async Task<ForgotPasswordResponse> SendPasswordResetEmailAsync(ForgotPasswordRequest forgotPasswordRequest, CancellationToken cancellationToken)
        {
            forgotPasswordRequest.ThrowIfNull("forgotPasswordRequest");
            var user = await UserData.GetUserAsync(forgotPasswordRequest.Username, cancellationToken);
            var link = _linkGenerator.GenerateUniqueLink();

            var sent = await _linkGenerator.SendResetPasswordLink(user, link, cancellationToken);

            await UserData.StoreResetPasswordLink(user.UserId, link, cancellationToken);

            return new ForgotPasswordResponse
            {
                LinkSent = sent
            };
        }

        [HttpPost("resetpassword")]
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var valid = await UserData.IsLinkValidAsync(request.Link, cancellationToken);

            if (!valid) throw new UnauthorizedAccessException();

            var user = await UserData.GetUserByLinkAsync(request.Link, cancellationToken);

            var response = await UserData.ResetUserPasswordAsync(user.UserId, request.NewPassword, cancellationToken);

            return response;
        }

        [HttpGet("checktoken")]
        public async Task<bool> CheckTokenAsync()
        {
            return await base.IsAuthenticatedAsync();
        }
    }
}
