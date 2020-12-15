using Microsoft.Extensions.Logging;
using System;

namespace DotNet.Logging.CustomProvider
{
    public class ColorConsoleLoggerConfiguration
    {
        public int EventId { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        public ConsoleColor Color { get; set; } = ConsoleColor.Green;
    }
}
