using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Database
{
    /// <summary>
    /// This class is responsible for executing commands against a database.
    /// </summary>
    public interface ICommandExecutor
    {
        /// <summary>
        /// Executes a command and returns <see cref="IEnumerable{T}"/> results.
        /// </summary>
        /// <typeparam name="T">a type to which to convert query results</typeparam>
        /// <param name="command">a command to execute</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        IEnumerable<T> Execute<T>(ICommand command) where T : class, new();
        /// <summary>
        /// Executes a command and returns the first row as an instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">the type to convert the query result.</typeparam>
        /// <param name="command">a command to execute</param>
        /// <returns>an instance of <typeparamref name="T"/></returns>
        T ExecuteSingle<T>(ICommand command) where T : class, new();
        /// <summary>
        /// Executes a command and returns the first column of the first row as an instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">the type to convert the query result.</typeparam>
        /// <param name="command">a command to execute</param>
        /// <returns>an instance of <typeparamref name="T"/></returns>
        T ExecuteScalar<T>(ICommand command);
        /// <summary>
        /// Executes a command and returns the number of affected records.
        /// </summary>
        /// <param name="command">a command to execute</param>
        /// <returns>the number of affected records</returns>
        int ExecuteNonQuery(ICommand command);
        /// <summary>
        /// Executes a command asynchronously and returns <see cref="IEnumerable{T}"/> results.
        /// </summary>
        /// <typeparam name="T">a type to which to convert query results</typeparam>
        /// <param name="command">a command to execute</param>
        /// <param name="cancellationToken">token used to cancel the action</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<T>> ExecuteAsync<T>(ICommand command, CancellationToken cancellationToken) where T : class, new();
        /// <summary>
        /// Executes a command asynchronously and returns the first row as an instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">the type to convert the query result.</typeparam>
        /// <param name="command">a command to execute</param>
        /// <param name="cancellationToken">token used to cancel the action</param>
        /// <returns>an instance of <typeparamref name="T"/></returns>
        Task<T> ExecuteSingleAsync<T>(ICommand command, CancellationToken cancellationToken) where T : class, new();
        /// <summary>
        /// Executes a command asynchronously and returns the first column of the first row as an instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">the type to convert the query result.</typeparam>
        /// <param name="command">a command to execute</param>
        /// <param name="cancellationToken">token used to cancel the action</param>
        /// <returns>an instance of <typeparamref name="T"/></returns>
        Task<T> ExecuteScalarAsync<T>(ICommand command, CancellationToken cancellationToken);
        /// <summary>
        /// Executes a command asynchronously and returns the number of affected records.
        /// </summary>
        /// <param name="command">a command to execute</param>
        /// <param name="cancellationToken">token used to cancel the action</param>
        /// <returns>the number of affected records</returns>
        Task<int> ExecuteNonQueryAsync(ICommand command, CancellationToken cancellationToken);
        /// <summary>
        /// Executes a command asynchronously and returns <see cref="IEnumerable{T}"/> results.
        /// </summary>
        /// <typeparam name="T">a type to which to convert query results</typeparam>
        /// <param name="command">a command to execute</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<T>> ExecuteAsync<T>(ICommand command) where T : class, new();
        /// <summary>
        /// Executes a command asynchronously and returns the first row as an instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">the type to convert the query result.</typeparam>
        /// <param name="command">a command to execute</param>
        /// <returns>an instance of <typeparamref name="T"/></returns>
        Task<T> ExecuteSingleAsync<T>(ICommand command) where T : class, new();
        /// <summary>
        /// Executes a command asynchronously and returns the first column of the first row as an instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">the type to convert the query result.</typeparam>
        /// <param name="command">a command to execute</param>
        /// <returns>an instance of <typeparamref name="T"/></returns>
        Task<T> ExecuteScalarAsync<T>(ICommand command);
        /// <summary>
        /// Executes a command asynchronously and returns the number of affected records.
        /// </summary>
        /// <param name="command">a command to execute</param>
        /// <returns>the number of affected records</returns>
        Task<int> ExecuteNonQueryAsync(ICommand command);
    }
}
