using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ServerManager.Rest.Database.Sqlite
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SqliteCommandFactory : IDbCommandFactory
    {
        public static SqliteCommandFactory Default => new SqliteCommandFactory();

        public ICommand BuildCommand(string commandText, CommandType commandType, IConnection connection, params IParameter[] parameters)
        {
            commandText.ThrowIfNull("commandText");
            connection.ThrowIfNull("connection");

            
            var command = new SqliteCommand
            {
                CommandText = commandText,
                CommandType = commandType,
                Connection = connection
            };

            if (parameters != null && parameters.Length > 0)
            {
                foreach (var param in parameters)
                    command.Parameters.Add(param);
            }

            return command;
        }
    }
}
