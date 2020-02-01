using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManager.Rest.Logging
{
    public interface ILogger
    {
        string Name { get; }
        /// <summary>
        /// Logs a pre-made message to the console/logfile.
        /// </summary>
        /// <param name="logLevel">level of severity</param>
        /// <param name="message">message to be logged</param>
        void Log(LogLevel logLevel, string message);

        /// <summary>
        /// Logs an exception to the console/logfile.
        /// </summary>
        /// <param name="logLevel">level of severity</param>
        /// <param name="ex">exception to be formatted and logged</param>
        void Log(LogLevel logLevel, Exception ex);

        /// <summary>
        /// logs a message to the console given a format and parameters.
        /// </summary>
        /// <param name="logLevel">level of severity</param>
        /// <param name="format">format to pass to string.Format</param>
        /// <param name="parameters">parameters to pass to string.Format</param>
        void Log(LogLevel logLevel, string format, params object[] parameters);
    }
}
