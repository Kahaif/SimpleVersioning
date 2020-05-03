using Microsoft.Extensions.Logging;
using System;

namespace SimpleVersioning.Logger
{
    public class Logger : ILogger
    {
        private readonly LoggerConfiguration configuration;
        private readonly string name;

        public Logger(string name, LoggerConfiguration configuration)
        {
            this.name = name;
            this.configuration = configuration;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == configuration.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            configuration.Callback?.Invoke($"{DateTime.Now} - {logLevel} - {eventId.Id} - {name} - {formatter(state, exception)}");
        }
    }
}
