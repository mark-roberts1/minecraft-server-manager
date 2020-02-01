using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManager.Rest.Database.Sqlite
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SqliteConnectionFactory : IDbConnectionFactory
    {
        public static SqliteConnectionFactory Default => new SqliteConnectionFactory();

        public IConnection BuildConnection(string connectionString)
        {
            return new SqliteConnection(connectionString);
        }
    }
}
