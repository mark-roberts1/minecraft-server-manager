using System.Data;

namespace ServerManager.Rest.Database
{
    /// <summary>
    /// A factory class used to construct implementations of <see cref="ICommand"/>
    /// </summary>
    public interface IDbCommandFactory
    {
        /// <summary>
        /// Constructs an <see cref="ICommand"/> instance using the given parameters.
        /// </summary>
        /// <param name="commandText">The text of the command to execute.</param>
        /// <param name="commandType">The type of command being executed.</param>
        /// <param name="connection">A database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>A command to be executed.</returns>
        ICommand BuildCommand(string commandText, CommandType commandType, IConnection connection, params IParameter[] parameters);
    }
}
