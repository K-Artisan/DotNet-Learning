using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DotNet.Configuration.OptionsPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            host.Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<ExampleService>();
                    services.AddTransient<ScopedService>();
                    services.AddTransient<MonitorService>();

                    services.AddHostedService<Worker>();

                    #region 转移到Configure方法中
                    //IServiceProvider serviceProvider = services.BuildServiceProvider();
                    //var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                    //var myoption = configuration.GetSection(nameof(MyOption)).Get<MyOption>();
                    //Console.WriteLine($"ConfigureService-->MyOption:{Newtonsoft.Json.JsonConvert.SerializeObject(myoption)}");


                    ////依赖注入
                    //services.Configure<MyOption>(configuration.GetSection(nameof(MyOption))); 
                    #endregion
                    services.AddOptions() //注册了Options模式的核心服务
                            .Configure<MyOption>(context.Configuration.GetSection(nameof(MyOption)));
                });
        }
    }
}
