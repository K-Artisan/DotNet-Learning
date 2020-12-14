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

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            //host.Services.C



            host.Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    IServiceProvider serviceProvider = services.BuildServiceProvider();
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                    var myoption = configuration.GetSection(nameof(MyOption)).Get<MyOption>();
                    Console.WriteLine($"ConfigureService-->MyOption:{Newtonsoft.Json.JsonConvert.SerializeObject(myoption)}");


                    //依赖注入
                    services.Configure<MyOption>(configuration.GetSection(nameof(MyOption)));

                    services.AddTransient<ExampleService>();
                    services.AddTransient<ScopedService>();
                    services.AddTransient<MonitorService>();

                    services.AddHostedService<Worker>();
                    
                });
        }
    }
}
