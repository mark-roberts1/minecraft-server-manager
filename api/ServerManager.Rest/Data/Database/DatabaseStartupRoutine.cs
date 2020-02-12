using Microsoft.Extensions.Configuration;
using ServerManager.Rest.Data;
using ServerManager.Rest.Data.Queries;
using ServerManager.Rest.Database;
using ServerManager.Rest.Dto;
using ServerManager.Rest.IO;
using ServerManager.Rest.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Database.Sqlite
{
    public class DatabaseStartupRoutine
    {
        private readonly string _dbFileName;
        private readonly string _connectionString;
        private readonly IDbCommandFactory _commandFactory;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IDiskOperator _diskOperator;
        private readonly IUserData _dataAccessLayer;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public DatabaseStartupRoutine(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _connectionString = configuration.ThrowIfNull("configuration").GetConnectionString("AppData").ThrowIfNull("connectionString");
            _dbFileName = _connectionString.Split(';')[0].Split('=')[1].Trim();

            _commandFactory = new SqliteCommandFactory();
            _connectionFactory = new SqliteConnectionFactory();
            _commandExecutor = new DatabaseCommandExecutor(new DataMapper());
            _diskOperator = new DiskOperator();

            _dataAccessLayer = new UserData(_commandFactory, _connectionFactory, _commandExecutor, loggerFactory, configuration);

            _logger = loggerFactory.GetLogger<DatabaseStartupRoutine>();
            _configuration = configuration;
        }

        public async Task Start()
        {
            _logger.Log(LogLevel.Info, "Starting database routine...");

            try
            {
                _logger.Log(LogLevel.Info, "Creating User table if not exists...");

                using var connection = _connectionFactory.BuildConnection(_connectionString);
                
                using (var cmd = _commandFactory.BuildCommand(SchemaCommands.CreateUserTable, CommandType.Text, connection))
                {
                    await _commandExecutor.ExecuteNonQueryAsync(cmd);
                }

                _logger.Log(LogLevel.Info, "Creating ResetPasswordLinkTable table if not exists...");

                using (var cmd = _commandFactory.BuildCommand(SchemaCommands.CreateResetPasswordLinkTable, CommandType.Text, connection))
                {
                    await _commandExecutor.ExecuteNonQueryAsync(cmd);
                }

                _logger.Log(LogLevel.Info, "Creating UserSession table if not exists...");

                using (var cmd = _commandFactory.BuildCommand(SchemaCommands.CreateUserSessionTable, CommandType.Text, connection))
                {
                    await _commandExecutor.ExecuteNonQueryAsync(cmd);
                }

                _logger.Log(LogLevel.Info, "Creating UserInvitationLink table if not exists...");

                using (var cmd = _commandFactory.BuildCommand(SchemaCommands.CreateUserInvtitationLinkTable, CommandType.Text, connection))
                {
                    await _commandExecutor.ExecuteNonQueryAsync(cmd);
                }

                _logger.Log(LogLevel.Info, "Creating DefaultProperties table if not exists...");

                using (var cmd = _commandFactory.BuildCommand(SchemaCommands.CreateDefaultPropertiesTable, CommandType.Text, connection))
                {
                    await _commandExecutor.ExecuteNonQueryAsync(cmd);
                }

                _logger.Log(LogLevel.Info, "Creating Template table if not exists...");

                using (var cmd = _commandFactory.BuildCommand(SchemaCommands.CreateTemplateTable, CommandType.Text, connection))
                {
                    await _commandExecutor.ExecuteNonQueryAsync(cmd);
                }

                _logger.Log(LogLevel.Info, "Creating Server table if not exists...");

                using (var cmd = _commandFactory.BuildCommand(SchemaCommands.CreateServerTable, CommandType.Text, connection))
                {
                    await _commandExecutor.ExecuteNonQueryAsync(cmd);
                }

                _logger.Log(LogLevel.Info, "Creating ServerProperties table if not exists...");

                using (var cmd = _commandFactory.BuildCommand(SchemaCommands.CreateServerPropertiesTable, CommandType.Text, connection))
                {
                    await _commandExecutor.ExecuteNonQueryAsync(cmd);
                }

                _logger.Log(LogLevel.Info, "Checking DefaultProperties...");

                int defaultPropertiesCount = 0;

                using (var cmd = _commandFactory.BuildCommand("SELECT COUNT(*) FROM DefaultProperties", CommandType.Text, connection))
                {
                    defaultPropertiesCount = await _commandExecutor.ExecuteScalarAsync<int>(cmd);
                }

                if (defaultPropertiesCount == 0)
                {
                    _logger.Log(LogLevel.Info, "Populating DefaultProperties...");

                    using (var cmd = _commandFactory.BuildCommand("INSERT INTO DefaultProperties ( Properties ) VALUES ( $Properties )", 
                        CommandType.Text, connection, DbParameter.From("$Properties", SchemaCommands.DefaultPropertiesValue)))
                    {
                        await _commandExecutor.ExecuteNonQueryAsync(cmd);
                    }
                }

                _logger.Log(LogLevel.Info, "Setting Server states to Stopped...");

                using (var cmd = _commandFactory.BuildCommand(ServerDbCommands.SetServerInitialState, CommandType.Text, connection))
                {
                    await _commandExecutor.ExecuteNonQueryAsync(cmd);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, "An exception interrupted the startup routine. Setup failed.");
                _logger.Log(LogLevel.Critical, ex);

                throw;
            }

            var newAdminUser = new CreateUserRequest
            {
                Username = _configuration.GetValue<string>("AppSettings:StarterAdminUser"),
                Password = _configuration.GetValue<string>("AppSettings:StarterAdminPass"),
                Email = _configuration.GetValue<string>("AppSettings:StarterAdminEmail")
            };

            var adminUser = await _dataAccessLayer.GetUserAsync(newAdminUser.Username, default);

            if (adminUser == null)
            {
                _logger.Log(LogLevel.Info, "Creating initial user...");

                var createResult = await _dataAccessLayer.CreateUserAsync(newAdminUser, default);

                if (!createResult.UserCreated)
                {
                    _logger.Log(LogLevel.Critical, "Unable to create the initial admin user. Setup failed.");
                    throw new Exception();
                }

                _logger.Log(LogLevel.Info, "Setting initial user role to admin...");

                adminUser = await _dataAccessLayer.GetUserAsync(newAdminUser.Username, default);

                var roleResult = await _dataAccessLayer.UpdateUserRole(adminUser.UserId, UserRole.Admin, default);

                if (!roleResult.RoleUpdated)
                {
                    _logger.Log(LogLevel.Critical, "Unable to set the role for the initial user. Setup failed.");
                    throw new Exception();
                }
            }

            _logger.Log(LogLevel.Info, "Setup has completed.");
        }


    }
}
