using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Database
{
    /// <summary>
    /// Represents an encountered exception while establishing a database connection
    /// </summary>
    public class ConnectionException : Exception
    {
        /// <summary>
        /// Creates an instance of <see cref="ConnectionException"/>
        /// </summary>
        /// <param name="message">A summary of the problem</param>
        public ConnectionException(string message) : base(message) { }
        /// <summary>
        /// Creates an instance of <see cref="ConnectionException"/>
        /// </summary>
        /// <param name="message">A summary of the problem</param>
        /// <param name="innerException">A reference to the underlying problem</param>
        public ConnectionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
