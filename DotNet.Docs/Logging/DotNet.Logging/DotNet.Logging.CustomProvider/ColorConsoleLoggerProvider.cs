using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DotNet.Logging.CustomProvider
{
    public class ColorConsoleLoggerProvider : ILoggerProvider
    {
        private readonly ColorConsoleLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string, ColorConsoleLogger> _loggers =
            new ConcurrentDictionary<string, ColorConsoleLogger>();

        public ColorConsoleLoggerProvider(ColorConsoleLoggerConfiguration config)
        {
            _config = config;
        }

        #region ILoggerProvider 成员

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new ColorConsoleLogger(name, _config));
        }
        #endregion

        public void Dispose() => _loggers.Clear();
    }
}
