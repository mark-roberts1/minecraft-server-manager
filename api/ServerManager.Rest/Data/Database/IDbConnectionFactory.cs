namespace ServerManager.Rest.Database
{
    /// <summary>
    /// A factory class used to construct implementations of <see cref="IConnection"/>
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Constructs a connection given a connection string.
        /// </summary>
        /// <param name="connectionString">information about a connection to a database</param>
        /// <returns>a connection</returns>
        IConnection BuildConnection(string connectionString);
    }
}
