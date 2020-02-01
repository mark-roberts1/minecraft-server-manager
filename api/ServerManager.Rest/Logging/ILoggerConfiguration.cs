using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManager.Rest.Logging
{
    /// <summary>
    /// Contract to implement to tell <see cref="LoggerFactory"/> how a logger should be configured.
    /// </summary>
    public interface ILoggerConfiguration
    {
        bool LogToFile { get; }
        NLog.LogLevel MinFileLogLevel { get; }
        bool LogToConsole { get; }
        NLog.LogLevel MinConsoleLogLevel { get; }
        bool UseArchive { get; }
        string GetLogFileName(string loggerName, DateTime logDate);
        int ArchiveAboveSize { get; }
        int MaxArchiveFiles { get; }
        string ArchiveDirectory { get; }
        string ArchiveFilePattern { get; }
        string LogPath { get; }
    }
}
