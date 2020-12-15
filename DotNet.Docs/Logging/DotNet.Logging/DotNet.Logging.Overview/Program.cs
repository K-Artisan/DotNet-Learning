using Microsoft.Extensions.Logging;

namespace DotNet.Logging.Overview
{
    /// <summary>
    /// 统一日志编程模式：
    /// 1.了解日志三大要素，及其使用方法
    /// 2.如何设置日志等级
    /// 3.如何设置日志过滤规则
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    //.SetMinimumLevel(LogLevel.Information)  //设置最小日志登记,不设置，默认为LogLevel.Information
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("DotNet.Logging.Overview", LogLevel.Information) //AddFilter 日志过滤规则
                    .AddConsole() //添加：ConsoleLoggerProvider，输出到控制台，
                    .AddDebug();  //添加:DebugLoggerProvider, 输出到VS调试器Debuger
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogDebug("LogDebug: log message"); //被过滤了，不会输出日志
            logger.LogInformation("LogInformation: log message");

        }
    }
}
