using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerManager.Rest.Data;
using ServerManager.Rest.Database;
using ServerManager.Rest.Dto;
using ServerManager.Rest.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LoginController : ApiController
    {
        private readonly ILinkGenerator _linkGenerator;
        
        public LoginController(IDataAccessLayer dataAccessLayer, ILinkGenerator linkGenerator)
            : base(dataAccessLayer)
        {
            _linkGenerator = linkGenerator.ThrowIfNull("linkGenerator");
        }

        [HttpPost("token")]
        public async Task<TokenResponse> LoginAsync([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var result = await DataAccessLayer.LoginAsync(loginRequest.ThrowIfNull("loginRequest"), cancellationToken);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException();
            }

            var token = _linkGenerator.GenerateUniqueLink();

            await DataAccessLayer.StoreUserSessionTokenAsync(loginRequest.Username, token, cancellationToken);

            return new TokenResponse
            {
                Token = token
            };
        }

        [HttpPost("forgotpassword")]
        public async Task<ForgotPasswordResponse> SendPasswordResetEmailAsync(ForgotPasswordRequest forgotPasswordRequest, CancellationToken cancellationToken)
        {
            forgotPasswordRequest.ThrowIfNull("forgotPasswordRequest");
            var user = await DataAccessLayer.GetUserAsync(forgotPasswordRequest.Username, cancellationToken);
            var link = _linkGenerator.GenerateUniqueLink();

            var sent = await _linkGenerator.SendResetPasswordLink(user, link, cancellationToken);

            await DataAccessLayer.StoreResetPasswordLink(user.UserId, link, cancellationToken);

            return new ForgotPasswordResponse
            {
                LinkSent = sent
            };
        }

        [HttpPost("resetpassword")]
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var valid = await DataAccessLayer.IsLinkValidAsync(request.Link, cancellationToken);

            if (!valid) throw new UnauthorizedAccessException();

            var user = await DataAccessLayer.GetUserByLinkAsync(request.Link, cancellationToken);

            var response = await DataAccessLayer.ResetUserPasswordAsync(user.UserId, request.NewPassword, cancellationToken);

            return response;
        }
    }
}
