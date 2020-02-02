using Microsoft.Extensions.Configuration;
using ServerManager.Rest.Database;
using ServerManager.Rest.Dto;
using ServerManager.Rest.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace ServerManager.Rest.Data
{
    public class DataAccessLayer : IDataAccessLayer
    {
        private readonly IDbCommandFactory _commandFactory;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICommandExecutor _commandExecutor;
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly IPasswordHasher<User> _passwordHasher;

        public DataAccessLayer(IDbCommandFactory commandFactory, 
            IDbConnectionFactory connectionFactory, 
            ICommandExecutor commandExecutor, 
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _commandFactory = commandFactory.ThrowIfNull("commandFactory");
            _connectionFactory = connectionFactory.ThrowIfNull("connectionFactory");
            _commandExecutor = commandExecutor.ThrowIfNull("commandExecutor");
            _logger = loggerFactory.ThrowIfNull("loggerFactory").GetLogger<DataAccessLayer>();
            _connectionString = configuration.ThrowIfNull("configuration").GetConnectionString("AppData").ThrowIfNull("connectionString");
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                Username = request.Username,
                UserRole = UserRole.Normal,
                IsLocked = false
            };

            var response = new CreateUserResponse();

            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.InsertUser, CommandType.Text, connection,
                    DbParameter.From("$Username", user.Username),
                    DbParameter.From("$Email", user.Email),
                    DbParameter.From("$UserRole", user.UserRole),
                    DbParameter.From("$IsLocked", user.IsLocked),
                    DbParameter.From("$PasswordHash", _passwordHasher.HashPassword(user, request.Password)));

                int affected = await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);

                response.UserCreated = affected == 1;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                response.UserCreated = false;
            }

            return response;
        }

        public async Task<DeleteUserResponse> DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            var response = new DeleteUserResponse();

            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.DeleteUser, CommandType.Text, connection, DbParameter.From("$UserId", userId));

                int affected = await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);

                response.UserDeleted = affected == 1;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                response.UserDeleted = false;
            }

            return response;
        }

        public async Task<User> GetUserAsync(string username, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.GetUserByUsername, CommandType.Text, connection, DbParameter.From("$Username", username));

                return await _commandExecutor.ExecuteSingleAsync<User>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task<User> GetUserAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.GetUserById, CommandType.Text, connection, DbParameter.From("$UserId", userId));

                return await _commandExecutor.ExecuteSingleAsync<User>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task<User> GetUserByLinkAsync(string link, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.GetUserByLink, CommandType.Text, connection, DbParameter.From("$Link", link));

                return await _commandExecutor.ExecuteSingleAsync<User>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task<User> GetUserBySessionTokenAsync(string token, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.GetUserBySessionToken, CommandType.Text, connection, DbParameter.From("$Token", token));

                return await _commandExecutor.ExecuteSingleAsync<User>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.GetUsers, CommandType.Text, connection);

                return await _commandExecutor.ExecuteAsync<User>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task<bool> IsLinkValidAsync(string link, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.IsLinkValid, CommandType.Text, connection, DbParameter.From("$Link", link));

                var dbLink = await _commandExecutor.ExecuteScalarAsync<string>(command, cancellationToken);

                return dbLink == link;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return false;
            }
        }

        public async Task<PasswordVerificationResult> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(loginRequest.Username, cancellationToken);
            var creds = await GetUserCredentialsAsync(loginRequest.Username, cancellationToken);

            var result = _passwordHasher.VerifyHashedPassword(user, creds.PasswordHash, loginRequest.Password);

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                await UpdateUserPasswordAsync(user.UserId, new UpdatePasswordRequest
                {
                    NewPassword = loginRequest.Password,
                    OriginalPassword = loginRequest.Password
                }, cancellationToken);

                return PasswordVerificationResult.Success;
            }

            return result;
        }

        private async Task<UserLogin> GetUserCredentialsAsync(string username, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.GetUsernamePasswordHash, CommandType.Text, connection, DbParameter.From("$Username", username));

                return await _commandExecutor.ExecuteSingleAsync<UserLogin>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task StoreInvitationLinkAsync(string email, string link, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.InsertInvitationLink,
                    CommandType.Text,
                    connection,
                    DbParameter.From("$Email", email),
                    DbParameter.From("$Link", link));

                await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task StoreResetPasswordLink(int userId, string link, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.InsertResetPasswordLink,
                    CommandType.Text,
                    connection,
                    DbParameter.From("$UserId", userId),
                    DbParameter.From("$Link", link));

                await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task<ToggleUserLockResponse> ToggleUserLockAsync(int userId, CancellationToken cancellationToken)
        {
            var response = new ToggleUserLockResponse();

            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.UpdateUserLock,
                    CommandType.Text,
                    connection,
                    DbParameter.From("$UserId", userId));

                response.IsUserLocked = await _commandExecutor.ExecuteScalarAsync<bool>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }

            return response;
        }

        public async Task<UpdateEmailResponse> UpdateUserEmailAsync(int userId, UpdateEmailRequest request, CancellationToken cancellationToken)
        {
            var response = new UpdateEmailResponse();

            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.UpdateUserEmail,
                    CommandType.Text,
                    connection,
                    DbParameter.From("$UserId", userId),
                    DbParameter.From("$Email", request.NewEmail));

                var affected = await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);

                response.EmailUpdated = affected == 1;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                response.EmailUpdated = false;
            }

            return response;
        }

        public async Task<UpdatePasswordResponse> UpdateUserPasswordAsync(int userId, UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            var response = new UpdatePasswordResponse();

            var user = await GetUserAsync(userId, cancellationToken);
            
            var result = await LoginAsync(new LoginRequest
            {
                Password = request.OriginalPassword,
                Username = user.Username
            }, cancellationToken);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException();
            }

            var newHash = _passwordHasher.HashPassword(user, request.NewPassword);

            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.UpdateUserPasswordHash,
                    CommandType.Text,
                    connection,
                    DbParameter.From("$UserId", userId),
                    DbParameter.From("$PasswordHash", newHash));

                var affected = await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);

                response.PasswordUpdated = affected == 1;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                response.PasswordUpdated = false;
            }

            return response;
        }

        public async Task<UpdateRoleResponse> UpdateUserRole(int userId, UserRole userRole, CancellationToken cancellationToken)
        {
            var response = new UpdateRoleResponse();

            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.UpdateUserRole,
                    CommandType.Text,
                    connection,
                    DbParameter.From("$UserId", userId),
                    DbParameter.From("$UserRole", userRole));

                var affected = await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);

                response.RoleUpdated = affected == 1;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                response.RoleUpdated = false;
            }

            return response;
        }

        public async Task StoreUserSessionTokenAsync(string username, string token, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(username, cancellationToken);

            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.InsertUserSessionToken, 
                    CommandType.Text, 
                    connection, 
                    DbParameter.From("$UserId", user.UserId),
                    DbParameter.From("$Token", token));

                await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task<ResetPasswordResponse> ResetUserPasswordAsync(int userId, string password, CancellationToken cancellationToken)
        {
            var response = new ResetPasswordResponse();

            var user = await GetUserAsync(userId, cancellationToken);

            var newHash = _passwordHasher.HashPassword(user, password);
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(UserCommands.UpdateUserPasswordHash,
                    CommandType.Text,
                    connection,
                    DbParameter.From("$UserId", userId),
                    DbParameter.From("$PasswordHash", newHash));

                var affected = await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);

                response.PasswordReset = affected == 1;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                response.PasswordReset = false;
            }

            return response;
        }
    }
}
