using Microsoft.Extensions.Logging;
using System;

namespace DotNet.Logging.CustomProvider
{
    public class ColorConsoleLogger : ILogger
    {

        private readonly string _name;
        private readonly ColorConsoleLoggerConfiguration _config;

        public ColorConsoleLogger(string name, ColorConsoleLoggerConfiguration config)
        {
            _name = name;
            _config = config;
        }


        #region  ILogger 成员

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _config.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (_config.EventId == 0 || _config.EventId == eventId.Id)
            {
                ConsoleColor originalColor = Console.ForegroundColor;

                Console.ForegroundColor = _config.Color;
                Console.WriteLine($"[{eventId.Id,2}: {logLevel,-12}]");

                Console.ForegroundColor = originalColor;
                Console.WriteLine($"     {_name} - {formatter(state, exception)}");
            }
        }

        #endregion
    }
}
