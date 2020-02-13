using Microsoft.Extensions.Configuration;
using ServerManager.Rest.Database;
using ServerManager.Rest.Dto;
using ServerManager.Rest.Logging;
using ServerManager.Rest.Management;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Data
{
    public class ServerData : IServerData
    {
        private readonly IDbCommandFactory _commandFactory;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICommandExecutor _commandExecutor;
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public ServerData(
            IDbCommandFactory commandFactory,
            IDbConnectionFactory connectionFactory,
            ICommandExecutor commandExecutor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _commandFactory = commandFactory.ThrowIfNull("commandFactory");
            _connectionFactory = connectionFactory.ThrowIfNull("connectionFactory");
            _commandExecutor = commandExecutor.ThrowIfNull("commandExecutor");
            _logger = loggerFactory.ThrowIfNull("loggerFactory").GetLogger<ServerData>();
            _connectionString = configuration.ThrowIfNull("configuration").GetConnectionString("AppData").ThrowIfNull("connectionString");
        }

        public async Task<AddTemplateResponse> AddTemplateAsync(AddTemplateRequest addTemplateRequest, CancellationToken cancellationToken)
        {
            var response = new AddTemplateResponse();

            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(ServerDbCommands.InsertTemplate, CommandType.Text, connection,
                    DbParameter.From("$Name", addTemplateRequest.Name),
                    DbParameter.From("$Description", addTemplateRequest.Description),
                    DbParameter.From("$Version", addTemplateRequest.Version),
                    DbParameter.From("$DownloadLink", addTemplateRequest.DownloadLink));

                response.TemplateId = await _commandExecutor.ExecuteScalarAsync<int>(command, cancellationToken);
                response.TemplateAdded = true;
            }
            catch (Exception e)
            {
                response.TemplateAdded = false;
                _logger.Log(LogLevel.Error, e);
                throw;
            }

            return response;
        }

        public async Task<CreateServerResponse> CreateAsync(CreateServerRequest createRequest, CancellationToken cancellationToken)
        {
            var response = new CreateServerResponse();

            try
            {
                using (var connection = _connectionFactory.BuildConnection(_connectionString))
                using (var command = _commandFactory.BuildCommand(ServerDbCommands.InsertServer, CommandType.Text, connection,
                    DbParameter.From("$Name", createRequest.Name),
                    DbParameter.From("$Description", createRequest.Description),
                    DbParameter.From("$Version", createRequest.Version)))
                {
                    response.ServerId = await _commandExecutor.ExecuteScalarAsync<int>(command, cancellationToken);
                }

                using (var connection = _connectionFactory.BuildConnection(_connectionString))
                using (var command = _commandFactory.BuildCommand(ServerDbCommands.InsertServerProperties, CommandType.Text, connection,
                    DbParameter.From("$ServerId", response.ServerId),
                    DbParameter.From("$Properties", createRequest.Properties.ToString())))
                {
                    await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);
                }

                response.Created = true;
            }
            catch (Exception e)
            {
                response.Created = false;
                _logger.Log(LogLevel.Error, e);
                throw;
            }

            return response;
        }

        public async Task<Template> GetTemplateAsync(string version, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(ServerDbCommands.SelectTemplateByVersion, CommandType.Text, connection,
                    DbParameter.From("$Version", version));

                return await _commandExecutor.ExecuteSingleAsync<Template>(command, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e);
                throw;
            }
        }

        public async Task<DeleteServerResponse> DeleteAsync(int serverId, CancellationToken cancellationToken)
        {
            var response = new DeleteServerResponse();

            try
            {
                int affected = 0;

                using (var connection = _connectionFactory.BuildConnection(_connectionString))
                using (var propertiesCommand = _commandFactory.BuildCommand(ServerDbCommands.DeleteServerProperties, CommandType.Text, connection,
                        DbParameter.From("$ServerId", serverId)))
                {
                    await _commandExecutor.ExecuteNonQueryAsync(propertiesCommand, cancellationToken);
                }

                using (var connection = _connectionFactory.BuildConnection(_connectionString))
                using (var serverCommand = _commandFactory.BuildCommand(ServerDbCommands.DeleteServer, CommandType.Text, connection,
                    DbParameter.From("$ServerId", serverId)))
                {
                    affected = await _commandExecutor.ExecuteNonQueryAsync(serverCommand, cancellationToken);
                }

                response.ServerDeleted = affected > 0;
            }
            catch (Exception e)
            {
                response.ServerDeleted = false;
                _logger.Log(LogLevel.Error, e);
                throw;
            }

            return response;
        }

        public async Task<ServerInfo> GetAsync(int serverId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);

                using var command = _commandFactory.BuildCommand(ServerDbCommands.SelectServer, CommandType.Text, connection,
                    DbParameter.From("$ServerId", serverId));

                return await _commandExecutor.ExecuteSingleAsync<ServerInfo>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task<Template> GetTemplateAsync(int templateId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);

                using var command = _commandFactory.BuildCommand(ServerDbCommands.SelectTemplate, CommandType.Text, connection,
                    DbParameter.From("$TemplateId", templateId));

                return await _commandExecutor.ExecuteSingleAsync<Template>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task<IEnumerable<ServerInfo>> ListAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);

                using var command = _commandFactory.BuildCommand(ServerDbCommands.ListServers, CommandType.Text, connection);

                return await _commandExecutor.ExecuteAsync<ServerInfo>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task<IEnumerable<Template>> ListTemplatesAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);

                using var command = _commandFactory.BuildCommand(ServerDbCommands.SelectTemplates, CommandType.Text, connection);

                return await _commandExecutor.ExecuteAsync<Template>(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                throw;
            }
        }

        public async Task<StartResponse> StartAsync(int serverId, CancellationToken cancellationToken)
        {
            var response = new StartResponse();

            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);

                using var command = _commandFactory.BuildCommand(ServerDbCommands.UpdateServerStatus, CommandType.Text, connection,
                    DbParameter.From("$ServerId", serverId),
                    DbParameter.From("$Status", ServerStatus.Started));

                await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                response.DidStart = false;
                response.Log = ex.PrintFull();
            }

            return response;
        }

        public async Task<bool> StopAsync(int serverId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);

                using var command = _commandFactory.BuildCommand(ServerDbCommands.UpdateServerStatus, CommandType.Text, connection,
                    DbParameter.From("$ServerId", serverId),
                    DbParameter.From("$Status", ServerStatus.Stopped));

                await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return false;
            }
        }

        public async Task<UpdateServerResponse> UpdateAsync(int serverId, UpdateServerRequest updateRequest, CancellationToken cancellationToken)
        {
            var response = new UpdateServerResponse();

            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);

                using var command1 = _commandFactory.BuildCommand(ServerDbCommands.UpdateServer, CommandType.Text, connection,
                    DbParameter.From("$ServerId", serverId),
                    DbParameter.From("$Name", updateRequest.NewName),
                    DbParameter.From("$Version", updateRequest.Version),
                    DbParameter.From("$Description", updateRequest.Description));

                await _commandExecutor.ExecuteNonQueryAsync(command1, cancellationToken);

                using var command2 = _commandFactory.BuildCommand(ServerDbCommands.UpdateServerProperties, CommandType.Text, connection,
                    DbParameter.From("$ServerId", serverId),
                    DbParameter.From("$Properties", updateRequest.NewProperties));

                await _commandExecutor.ExecuteNonQueryAsync(command2, cancellationToken);

                response.Updated = true;
            }
            catch (Exception ex)
            {
                response.Updated = false;
                _logger.Log(LogLevel.Error, ex);
            }

            return response;
        }

        public async Task<UpdateTemplateResponse> UpdateTemplateAsync(UpdateTemplateRequest updateTemplateRequest, CancellationToken cancellationToken)
        {
            var response = new UpdateTemplateResponse();

            try
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(ServerDbCommands.UpdateTemplate, CommandType.Text, connection,
                    DbParameter.From("$Name", updateTemplateRequest.Name),
                    DbParameter.From("$Description", updateTemplateRequest.Description),
                    DbParameter.From("$Version", updateTemplateRequest.Version),
                    DbParameter.From("$DownloadLink", updateTemplateRequest.DownloadLink),
                    DbParameter.From("$TemplateId", updateTemplateRequest.TemplateId));

                await _commandExecutor.ExecuteNonQueryAsync(command, cancellationToken);
                
                response.Updated = true;
            }
            catch (Exception ex)
            {
                response.Updated = false;
                _logger.Log(LogLevel.Error, ex);
            }

            return response;
        }

        public async Task<ServerPropertyList> GetDefaultPropertiesAsync(CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(ServerDbCommands.SelectDefaultProperties, CommandType.Text, connection);

            return (await _commandExecutor.ExecuteSingleAsync<DefaultProperties>(command, cancellationToken)).Properties;
        }

        private class DefaultProperties
        {
            public ServerPropertyList Properties { get; set; }
        }
    }
}
