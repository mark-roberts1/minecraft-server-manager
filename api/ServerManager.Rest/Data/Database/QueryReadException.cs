using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Database
{
    /// <summary>
    /// Represents an encountered exception while reading a query stored in an embedded resource
    /// </summary>
    public class QueryReadException : Exception
    {
        /// <summary>
        /// The name of the query causing the issue
        /// </summary>
        public string QueryName { get; }

        /// <summary>
        /// Creates an instance of <see cref="QueryReadException"/>
        /// </summary>
        /// <param name="queryName">The name of the query causing the issue</param>
        /// <param name="message">A summary of the problem</param>
        /// <param name="ex">A reference to the underlying problem</param>
        public QueryReadException(string queryName, string message, Exception ex = null)
            : base(message, ex)
        {
            QueryName = queryName.EnsureSqlFile();
        }
    }
}
