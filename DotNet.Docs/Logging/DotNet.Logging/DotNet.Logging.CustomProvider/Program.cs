using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace DotNet.Logging.CustomProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                ILoggerFactory loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                ILogger logger = loggerFactory.CreateLogger<Program>();

                logger.LogInformation("this a [Info] message form customProvider");
                logger.LogError("this a [Error] message form customProvider");
                logger.LogWarning("this a [Warning] message form customProvider");
            }

            host.Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                    builder.ClearProviders()
                        .AddProvider(
                            new ColorConsoleLoggerProvider(
                                new ColorConsoleLoggerConfiguration
                                {
                                    LogLevel = LogLevel.Error,
                                    Color = ConsoleColor.Red
                                }))
                        .AddColorConsoleLogger()
                        .AddColorConsoleLogger(configuration =>
                        {
                            configuration.LogLevel = LogLevel.Warning;
                            configuration.Color = ConsoleColor.DarkMagenta;
                        }));
    }
}
