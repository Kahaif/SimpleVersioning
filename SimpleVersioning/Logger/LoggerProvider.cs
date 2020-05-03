using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace SimpleVersioning.Logger
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, Logger> loggers = new ConcurrentDictionary<string, Logger>();
        private readonly LoggerConfiguration config;

        public LoggerProvider(LoggerConfiguration config)
        {
            this.config = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new Logger(name, config));
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }
}
