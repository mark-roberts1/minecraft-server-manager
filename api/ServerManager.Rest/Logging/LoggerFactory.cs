using ServerManager.Rest.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManager.Rest.Logging
{
    /// <summary>
    /// This class is best used as a singleton. NLog loggers and the LogManager are static resources and only need to be constructed once.
    /// </summary>
    public class LoggerFactory : ILoggerFactory
    {
        private readonly NLog.Targets.FileTarget _fileTarget;
        private readonly ILoggerConfiguration _loggerConfiguration;
        private readonly IDiskOperator _diskOperator;

        public LoggerFactory(ILoggerConfiguration loggerConfiguration = null, IDiskOperator diskOperator = null)
        {
            _diskOperator = diskOperator ?? new DiskOperator();
            _loggerConfiguration = loggerConfiguration ?? LoggerConfiguration.Default;

            var config = new NLog.Config.LoggingConfiguration();

            _fileTarget = new NLog.Targets.FileTarget
            {
                FileName = _diskOperator.CombinePaths(_loggerConfiguration.LogPath, _loggerConfiguration.GetLogFileName(nameof(LoggerFactory), DateTime.Now))
            };

            if (_loggerConfiguration.UseArchive)
            {
                _fileTarget.ArchiveAboveSize = _loggerConfiguration.ArchiveAboveSize;
                _fileTarget.ArchiveFileName = _diskOperator.CombinePaths(_loggerConfiguration.ArchiveDirectory, _loggerConfiguration.ArchiveFilePattern);
                _fileTarget.ArchiveNumbering = NLog.Targets.ArchiveNumberingMode.Rolling;
                _fileTarget.MaxArchiveFiles = _loggerConfiguration.MaxArchiveFiles;
            }

            if (_loggerConfiguration.LogToFile)
            {
                config.LoggingRules.Add(new NLog.Config.LoggingRule("*", _loggerConfiguration.MinFileLogLevel, _fileTarget));
            }

            if (_loggerConfiguration.LogToConsole)
            {
                var consoleTarget = new NLog.Targets.ConsoleTarget();
                config.LoggingRules.Add(new NLog.Config.LoggingRule("*", _loggerConfiguration.MinConsoleLogLevel, consoleTarget));
            }

            NLog.LogManager.Configuration = config;
        }

        public ILogger GetLogger<T>()
        {
            var type = typeof(T);

            NLog.ILogger nlogger;

            string name = null;

            if (type.IsGenericType)
            {
                name = BuildGenericTypeName(type);
            }
            else
            {
                name = type.Name;
            }

            // will increment log file name as necessary
            _fileTarget.FileName = _diskOperator.CombinePaths(_loggerConfiguration.LogPath, _loggerConfiguration.GetLogFileName(name, DateTime.Now));

            nlogger = NLog.LogManager.GetLogger(name);

            return new Logger(nlogger);
        }

        private string BuildGenericTypeName(Type type)
        {
            Type[] typeArgs = type.GetGenericArguments();

            var name = type.Name.Replace($"`{typeArgs.Length}", "<");

            for (int i = 0; i < typeArgs.Length; i++)
            {
                var subtype = typeArgs[i];

                name = $"{name}{subtype.Name}";

                if (i < typeArgs.Length - 1)
                {
                    name += ", ";
                }
            }

            return $"{name}>";
        }
    }
}
