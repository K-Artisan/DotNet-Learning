[TOC]

# 参考文档

https://docs.microsoft.com/en-us/dotnet/core/extensions/logging

# 统一日志编程模式

为了整合微软自身和第三方日志框架(如：Log4Net，NLog，Serilog)，微软创建了一个用来提供统一的日志编程模式的日志框架。

统一日志编程模式主要涉及由`ILogger`，`ILoggerFactory`, `ILoggerProvider` 表示的3个核心对象，这3个核心对象以及它们之间的关系和使用它们的大概流程是：

​		**应用程序通过`ILoggerFactory`创建`ILogger`对象来记录日志，而`ILoggerProvider`则完成针对相应渠道(输出到哪里：控制台，文件)的日志输出**



【示例：1-1.DotNet.Logging.Overview】

Program.cs

```C#
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
                    //.SetMinimumLevel(LogLevel.Information) //设置最小日志登记,不设置，默认为LogLevel.Information
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

```



# 日志范围

日志范围的作用是，在日志范围内输出的日志都额外输出Scope指定的字符串，一般会带有一个唯一Guid值用于标识日志的范围，

通过唯一Guid值可以筛选出对应日志。



【示例： 1-2.DotNet.Logging.Scope】

 本示例演示：日志范围的使用

```C#
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

```

- 开启日志范围

  ```C#
   .AddConsole(options =>
                      {
                          options.IncludeScopes = true; //开启日志范围上下文的特性
                      });
  ```

  

- 使用日志范围

  ```C#
  using (logger.BeginScope($"This a Transation, and it' Id is:[{ Guid.NewGuid().ToString()[^5..]}]"))
  {
      logger.LogInformation("...");
      //.....
      logger.LogInformation("...");
  }
         
  ```

  

输出：

```powershell
info: DotNet.Logging.Overview.Program[0]
      Other message-1
      
info: DotNet.Logging.Overview.Program[0]
      => This a Transation, and it' Id is:[06604]
      Transation option-1 complected at 00:00:01.0300564
info: DotNet.Logging.Overview.Program[0]
      => This a Transation, and it' Id is:[06604]
      Transation option-2 complected at00:00:02.0454977
info: DotNet.Logging.Overview.Program[0]
      => This a Transation, and it' Id is:[06604]
      Transation option-3 complected at 00:00:03.0503832
      
info: DotNet.Logging.Overview.Program[0]
      Other message-2

```

相同日志范围内的每条日志额外输出字符串：

```powershell
=> This a Transation, and it' Id is:[06604]
```

这样我们就**,可以用这个 [06604] 筛选出同一个事务内的日志。**



# LoggerMessage

定制输出模板

（略）



# 日志模型详解

日志模型三要素：

- ILogger
- ILoggerProvider
- ILoggerFactory



ILoggerProvider对象和ILoggerFactory对象都是ILogger的创建者，而ILoggerProvider对象会注册到ILoggerFactory对象中。



ILoggerFactory中的CreateLogger方法创建的ILogger是logger分发器

而

ILoggerProvider中的CreateLogger方法创建的ILogger是实际的日志输出者



【示例：1-3.DotNet.Logging.CustomProvider】



`ColorConsoleLoggerConfiguration.cs`

```C#
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

```



`ColorConsoleLogger .cs`

```C#
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

```



`ColorConsoleLoggerProvider.cs`

```C#
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

```



`ColorConsoleLoggerExtensions.cs`

```C#
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

```



`Progarm.cs`

```C#
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

```

