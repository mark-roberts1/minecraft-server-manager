using System;

namespace ServerManager.Rest.Logging
{
    public class Logger : ILogger
    {
        private readonly NLog.ILogger _logger;
        private static readonly object _syncLock = new object();

        public string Name => _logger.Name;

        public Logger(NLog.ILogger logger)
        {
            _logger = logger;
        }

        public void Log(LogLevel logLevel, string message)
        {
            lock (_syncLock)
            {
                switch (logLevel)
                {
                    case LogLevel.Info:
                        _logger.Info(message);
                        break;
                    case LogLevel.Warning:
                        _logger.Warn(message);
                        break;
                    case LogLevel.Error:
                        _logger.Error(message);
                        break;
                    case LogLevel.Critical:
                        _logger.Fatal(message);
                        break;
                }
            }
        }

        public void Log(LogLevel logLevel, Exception ex)
        {
            lock (_syncLock)
            {
                switch (logLevel)
                {
                    case LogLevel.Info:
                        _logger.Info(ex.PrintFull());
                        break;
                    case LogLevel.Warning:
                        _logger.Warn(ex.PrintFull());
                        break;
                    case LogLevel.Error:
                        _logger.Error(ex.PrintFull());
                        break;
                    case LogLevel.Critical:
                        _logger.Fatal(ex.PrintFull());
                        break;
                }
            }
        }

        public void Log(LogLevel logLevel, string format, params object[] parameters)
        {
            lock (_syncLock)
            {
                switch (logLevel)
                {
                    case LogLevel.Info:
                        _logger.Info(string.Format(format, parameters));
                        break;
                    case LogLevel.Warning:
                        _logger.Warn(string.Format(format, parameters));
                        break;
                    case LogLevel.Error:
                        _logger.Error(string.Format(format, parameters));
                        break;
                    case LogLevel.Critical:
                        _logger.Fatal(string.Format(format, parameters));
                        break;
                }
            }
        }
    }
}
