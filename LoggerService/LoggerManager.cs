using Contracts;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger _loggerManager = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message)
        => _loggerManager.Debug(message);

        public void LogError(string message)
        => _loggerManager.Error(message);
        public void LogInfo(string message)
        => _loggerManager.Info(message);


        public void LogWarn(string message)
        => _loggerManager.Warn(message);
    }
}
