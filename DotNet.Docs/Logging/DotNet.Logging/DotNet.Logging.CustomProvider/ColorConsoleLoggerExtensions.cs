using Microsoft.Extensions.Logging;
using System;

namespace DotNet.Logging.CustomProvider
{
    public static class ColorConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddColorConsoleLogger(this ILoggingBuilder builder)
        {
            return builder.AddColorConsoleLogger(new ColorConsoleLoggerConfiguration());
        }
           

        public static ILoggingBuilder AddColorConsoleLogger(this ILoggingBuilder builder,
            Action<ColorConsoleLoggerConfiguration> configure)
        {
            var config = new ColorConsoleLoggerConfiguration();
            configure(config);

            return builder.AddColorConsoleLogger(config);
        }

        public static ILoggingBuilder AddColorConsoleLogger(this ILoggingBuilder builder,
            ColorConsoleLoggerConfiguration config)
        {
            builder.AddProvider(new ColorConsoleLoggerProvider(config));
            return builder;
        }
    }
}
