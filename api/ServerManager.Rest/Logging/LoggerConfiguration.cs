using System;
using System.Collections.Generic;
using System.Text;
using ServerManager.Rest.IO;
using NLog;

namespace ServerManager.Rest.Logging
{
    public class LoggerConfiguration : ILoggerConfiguration
    {
        public bool LogToFile { get; set; }

        public NLog.LogLevel MinFileLogLevel { get; set; }

        public bool LogToConsole { get; set; }

        public NLog.LogLevel MinConsoleLogLevel { get; set; }

        public bool UseArchive { get; set; }

        public int ArchiveAboveSize { get; set; }

        public int MaxArchiveFiles { get; set; }

        public string ArchiveDirectory { get; set; }

        public string ArchiveFilePattern { get; set; }

        public string LogPath { get; set; }

        public virtual string GetLogFileName(string loggerName, DateTime logDate)
        {
            return $"Trace_{logDate.Year}{logDate.Month.ToString("d2")}{logDate.Day.ToString("d2")}.txt";
        }

        private static LoggerConfiguration _default;

        public static LoggerConfiguration Default
        {
            get
            {
                if (_default == null)
                {
                    var diskOperator = new DiskOperator();
                    var logPath = diskOperator.CombinePaths(diskOperator.GetAppDirectory(), "logs");
                    var archivePath = diskOperator.CombinePaths(logPath, "archive");

                    _default = new LoggerConfiguration
                    {
                        LogPath = logPath,
                        ArchiveDirectory = archivePath,
                        LogToFile = true,
                        LogToConsole = true,
                        UseArchive = true,
                        ArchiveAboveSize = 5000000,
                        MaxArchiveFiles = 2,
                        ArchiveFilePattern = "log.{####}.txt",
                        MinFileLogLevel = NLog.LogLevel.Info,
                        MinConsoleLogLevel = NLog.LogLevel.Info
                    };
                }

                return _default;
            }
        }
    }
}
