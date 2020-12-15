using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DotNet.Logging.Overview
{
    /// <summary>
    /// 本示例演示：
    /// 日志范围的使用
    /// 
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    //.SetMinimumLevel(LogLevel.Information) //设置最小日志登记,不设置，默认为LogLevel.Information
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("DotNet.Logging.Overview", LogLevel.Debug) //AddFilter 日志过滤规则
                    .AddConsole(options =>
                    {
                        options.IncludeScopes = true; //开启日志范围上下文的特性
                    });
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Other message-1");

            using (logger.BeginScope($"This a Transation, and it' Id is:[{ Guid.NewGuid().ToString()[^5..]}]"))
            {
                var stopwatch = Stopwatch.StartNew();

                await Task.Delay(1000);
                logger.LogInformation($"Transation option-1 complected at {stopwatch.Elapsed}");

                await DoTransationOption2(logger, stopwatch);

                await Task.Delay(1000);
                logger.LogInformation($"Transation option-3 complected at {stopwatch.Elapsed}");

            }

            logger.LogInformation("Other message-2");
        }

        static async Task DoTransationOption2(ILogger logger, Stopwatch stopwatch) {

            await Task.Delay(1000);
            logger.LogInformation($"Transation option-2 complected at{stopwatch.Elapsed}");
        }
    }
}
