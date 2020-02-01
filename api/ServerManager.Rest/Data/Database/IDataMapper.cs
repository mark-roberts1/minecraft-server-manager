using System.Collections.Generic;
using System.Data;

namespace ServerManager.Rest.Database
{
    /// <summary>
    /// Class used for mapping data from the database to concrete classes
    /// </summary>
    public interface IDataMapper
    {
        /// <summary>
        /// Reads a single result from <see cref="IDataReader"/>
        /// </summary>
        /// <typeparam name="T">Type to which to convert</typeparam>
        /// <param name="reader">Data reader containing data to convert</param>
        T MapSingle<T>(IDataReader reader) where T : new();
        /// <summary>
        /// Reads a collection of results from <see cref="IDataReader"/>
        /// </summary>
        /// <typeparam name="T">Type to which to convert</typeparam>
        /// <param name="reader">Data reader containing data to convert</param>
        IEnumerable<T> Map<T>(IDataReader reader) where T : new();
    }
}
