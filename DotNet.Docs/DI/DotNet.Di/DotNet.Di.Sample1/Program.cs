using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

/// <summary>
/// 本例展示：
/// 通过DI实现IOC
/// </summary>
namespace DotNet.Di.Sample1
{
    class Program
    {
        static Task Main(string[] args) =>
            CreateHostBuilder(args).Build().RunAsync();

        static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<Worker>()
                        .AddScoped<IMessageWriter, MessageWriter>();
                        //.AddScoped<IMessageWriter, LoggingMessageWriter>(); //通过DI实现IOC
                    
            });

    }
}
