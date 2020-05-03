using Microsoft.Extensions.Logging;
using System;

namespace SimpleVersioning.Logger
{
    public class LoggerConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        /// <summary>
        /// Executed once the Logger has formatted the text to log.
        /// </summary>
        public Action<string> Callback {get; set;}
    }

}
