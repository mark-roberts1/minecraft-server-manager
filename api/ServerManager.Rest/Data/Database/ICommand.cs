using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Database
{
    /// <summary>
    /// A command to execute against a database
    /// </summary>
    public interface ICommand : IDisposable
    {
        /// <summary>
        /// The command to be executed.
        /// </summary>
        string CommandText { get; set; }
        /// <summary>
        /// The type of command being executed.
        /// </summary>
        CommandType CommandType { get; set; }
        /// <summary>
        /// The database connection.
        /// </summary>
        IConnection Connection { get; set; }
        /// <summary>
        /// the parameter name/values for parameterizing the query.
        /// </summary>
        IList<IParameter> Parameters { get; }
        /// <summary>
        /// executes a command asynchronously, and returns an <see cref="IDataReader"/> instance to read the data.
        /// </summary>
        /// <param name="cancellationToken">token to cancel the action</param>
        /// <returns>data reader</returns>
        Task<IDataReader> ExecuteReaderAsync(CancellationToken cancellationToken);
        /// <summary>
        /// executes a command asynchronously, and returns the number of affected records.
        /// </summary>
        /// <param name="cancellationToken">token to cancel the action</param>
        /// <returns>number of affected records.</returns>
        Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken);
        /// <summary>
        /// executes a command asynchronously, and returns the first column of the first row of results.
        /// </summary>
        /// <param name="cancellationToken">token to cancel the action</param>
        /// <returns>the first column of the first row of results</returns>
        Task<object> ExecuteScalarAsync(CancellationToken cancellationToken);
        /// <summary>
        /// executes a command asynchronously, and returns an <see cref="IDataReader"/> instance to read the data.
        /// </summary>
        /// <returns>data reader</returns>
        Task<IDataReader> ExecuteReaderAsync();
        /// <summary>
        /// executes a command asynchronously, and returns the number of affected records.
        /// </summary>
        /// <returns>number of affected records.</returns>
        Task<int> ExecuteNonQueryAsync();
        /// <summary>
        /// executes a command asynchronously, and returns the first column of the first row of results.
        /// </summary>
        /// <returns>the first column of the first row of results</returns>
        Task<object> ExecuteScalarAsync();
        /// <summary>
        /// executes a command, and returns an <see cref="IDataReader"/> instance to read the data.
        /// </summary>
        /// <returns>data reader</returns>
        IDataReader ExecuteReader();
        /// <summary>
        /// executes a command, and returns the number of affected records.
        /// </summary>
        /// <returns>number of affected records.</returns>
        int ExecuteNonQuery();
        /// <summary>
        /// executes a command, and returns the first column of the first row of results.
        /// </summary>
        /// <returns>the first column of the first row of results</returns>
        object ExecuteScalar();
    }
}
