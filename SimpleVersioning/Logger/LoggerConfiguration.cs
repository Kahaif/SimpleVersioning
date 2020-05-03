using Microsoft.Extensions.Logging;
using System;

namespace SimpleVersioning.Logger
{
    public class LoggerConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        public Action<string> Callback {get; set;}
    }

}
