using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Database
{
    /// <summary>
    /// Used to queries against a database
    /// </summary>
    public class DatabaseCommandExecutor : ICommandExecutor
    {
        private readonly IDataMapper _dataMapper;

        /// <summary>
        /// Constructs an instance of <see cref="DatabaseCommandExecutor"/>
        /// </summary>
        /// <param name="dataMapper">Maps data to models from a DataReader</param>
        public DatabaseCommandExecutor(IDataMapper dataMapper)
        {
            _dataMapper = dataMapper.ThrowIfNull("dataMapper");
        }

        /// <inheritdoc/>
        public IEnumerable<T> Execute<T>(ICommand command) where T : class, new()
        {
            command.ThrowIfNull("command");

            IEnumerable<T> results;

            command.Connection.Open();

            using (var reader = command.ExecuteReader())
            {
                results = _dataMapper.Map<T>(reader);
            }

            return results;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> ExecuteAsync<T>(ICommand command, CancellationToken cancellationToken) where T : class, new()
        {
            command.ThrowIfNull("command");

            IEnumerable<T> results;

            await command.Connection.OpenAsync(cancellationToken);
            using (var reader = await command.ExecuteReaderAsync(cancellationToken))
            {
                results = _dataMapper.Map<T>(reader);
            }

            return results;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> ExecuteAsync<T>(ICommand command) where T : class, new()
        {
            command.ThrowIfNull("command");

            IEnumerable<T> results;

            await command.Connection.OpenAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                results = _dataMapper.Map<T>(reader);
            }

            return results;
        }

        /// <inheritdoc/>
        public int ExecuteNonQuery(ICommand command)
        {
            command.ThrowIfNull("command");

            command.Connection.Open();

            return command.ExecuteNonQuery();
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteNonQueryAsync(ICommand command, CancellationToken cancellationToken)
        {
            command.ThrowIfNull("command");

            await command.Connection.OpenAsync(cancellationToken);
            return await command.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteNonQueryAsync(ICommand command)
        {
            command.ThrowIfNull("command");

            await command.Connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }

        /// <inheritdoc/>
        public T ExecuteScalar<T>(ICommand command)
        {
            command.ThrowIfNull("command");

            command.Connection.Open();

            object scalarValue = command.ExecuteScalar();

            return (T)Convert.ChangeType(scalarValue, typeof(T));
        }

        /// <inheritdoc/>
        public async Task<T> ExecuteScalarAsync<T>(ICommand command, CancellationToken cancellationToken)
        {
            command.ThrowIfNull("command");

            await command.Connection.OpenAsync(cancellationToken);
            object scalarValue = await command.ExecuteScalarAsync(cancellationToken);

            return (T)Convert.ChangeType(scalarValue, typeof(T));
        }

        /// <inheritdoc/>
        public async Task<T> ExecuteScalarAsync<T>(ICommand command)
        {
            command.ThrowIfNull("command");

            await command.Connection.OpenAsync();
            object scalarValue = await command.ExecuteScalarAsync();

            return (T)Convert.ChangeType(scalarValue, typeof(T));
        }

        /// <inheritdoc/>
        public T ExecuteSingle<T>(ICommand command) where T : class, new()
        {
            command.ThrowIfNull("command");

            T result;

            command.Connection.Open();

            using (var reader = command.ExecuteReader())
            {
                result = _dataMapper.MapSingle<T>(reader);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<T> ExecuteSingleAsync<T>(ICommand command, CancellationToken cancellationToken) where T : class, new()
        {
            command.ThrowIfNull("command");

            T result;

            await command.Connection.OpenAsync(cancellationToken);
            using (var reader = await command.ExecuteReaderAsync(cancellationToken))
            {
                result = _dataMapper.MapSingle<T>(reader);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<T> ExecuteSingleAsync<T>(ICommand command) where T : class, new()
        {
            command.ThrowIfNull("command");

            T result;

            await command.Connection.OpenAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                result = _dataMapper.MapSingle<T>(reader);
            }

            return result;
        }
    }
}
