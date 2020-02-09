using Microsoft.AspNetCore.Identity;
using ServerManager.Rest.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Data
{
    public interface IUserData
    {
        Task<PasswordVerificationResult> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);
        Task<bool> LogOutAsync(string token, CancellationToken cancellationToken);
        Task StoreUserSessionTokenAsync(string username, string token, CancellationToken cancellationToken);
        Task<User> GetUserAsync(string username, CancellationToken cancellationToken);
        Task<User> GetUserAsync(int userId, CancellationToken cancellationToken);
        Task<User> GetUserByLinkAsync(string link, CancellationToken cancellationToken);
        Task<User> GetUserBySessionTokenAsync(string token, CancellationToken cancellationToken);
        Task StoreResetPasswordLink(int userId, string link, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken);
        Task<UpdatePasswordResponse> UpdateUserPasswordAsync(int userId, UpdatePasswordRequest request, CancellationToken cancellationToken);
        Task<UpdateEmailResponse> UpdateUserEmailAsync(int userId, UpdateEmailRequest request, CancellationToken cancellationToken);
        Task StoreInvitationLinkAsync(string email, string link, CancellationToken cancellationToken);
        Task<bool> IsLinkValidAsync(string link, CancellationToken cancellationToken);
        Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
        Task<DeleteUserResponse> DeleteUserAsync(int userId, CancellationToken cancellationToken);
        Task<UpdateRoleResponse> UpdateUserRole(int userId, UserRole userRole, CancellationToken cancellationToken);
        Task<ToggleUserLockResponse> ToggleUserLockAsync(int userId, CancellationToken cancellationToken);
        Task<ResetPasswordResponse> ResetUserPasswordAsync(int userId, string password, CancellationToken cancellationToken);
        
    }
}
