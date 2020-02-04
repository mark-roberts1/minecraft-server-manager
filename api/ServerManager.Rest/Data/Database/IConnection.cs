using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Database
{
    /// <summary>
    /// Represents a connection to a database.
    /// </summary>
    public interface IConnection : IDisposable
    {
        /// <summary>
        /// Determines if connection is open.
        /// </summary>
        bool IsOpen { get; }
        /// <summary>
        /// Asynchronously opens a cancellable connection to the database.
        /// </summary>
        /// <param name="cancellationToken">token used to cancel the action.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ConnectionException"></exception>
        Task OpenAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Asynchronously opens a connection to the database.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ConnectionException"></exception>
        Task OpenAsync();
        /// <summary>
        /// Asynchronously opens a connection to the database.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ConnectionException"></exception>
        void Open();
        /// <summary>
        /// Closes the Connection.
        /// </summary>
        void Close();
        /// <summary>
        /// Changes the database targeted on the connection.
        /// </summary>
        /// <param name="database">Name of the database to target</param>
        void ChangeDatabase(string database);
    }
}
