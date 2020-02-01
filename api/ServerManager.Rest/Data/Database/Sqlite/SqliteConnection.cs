using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Database.Sqlite
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SqliteConnection : IConnection
    {
        private bool disposed;
        public SQLiteConnection Connection { get; }

        public SqliteConnection(string connectionString)
        {
            Connection = new SQLiteConnection(connectionString);
        }

        public SqliteConnection(SQLiteConnection connection)
        {
            Connection = connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                Connection.Dispose();
                disposed = true;
            }
        }

        public async Task OpenAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Connection.OpenAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ConnectionException($"An error occurred while opening a connection to {Connection.Database} on {Connection.DataSource}", ex);
            }
        }

        public async Task OpenAsync()
        {
            try
            {
                await Connection.OpenAsync();
            }
            catch (Exception ex)
            {
                throw new ConnectionException($"An error occurred while opening a connection to {Connection.Database} on {Connection.DataSource}", ex);
            }
        }

        public void Open()
        {
            try
            {
                Connection.Open();
            }
            catch (Exception ex)
            {
                throw new ConnectionException($"An error occurred while opening a connection to {Connection.Database} on {Connection.DataSource}", ex);
            }
        }

        public void Close()
        {
            Connection.Close();
        }

        public void ChangeDatabase(string database)
        {
            Connection.ChangeDatabase(database);
        }
    }
}
