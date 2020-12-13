using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DotNet.Configuration.CustomProvider
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            // Application code should start here.

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, configuration) =>
                {
                    configuration.Sources.Clear();

                    configuration.AddEntityConfiguration(
                        options => options.UseInMemoryDatabase("InMemoryDb")); //EF Core 内存数据库

                    IConfigurationRoot configurationRoot = configuration.Build();


                    foreach ((string key, string value) in
                        configurationRoot.AsEnumerable().Where(t => t.Value is not null))
                    {
                        Console.WriteLine($"{key}={value}");
                    }

                    //---------------------获取配置值的方法--------------------------------
                    //GetSection(xxx:x1)
                    var myOption1 = configurationRoot.GetSection("EndpointId")?.Value;
                    Console.WriteLine($"EndpointId: {myOption1}");

                    //GetSection(xxx)[x1]
                    var myOption2 = configurationRoot["DisplayLabel"];
                    Console.WriteLine($"DisplayLabel: {myOption2}");

                    //创建对象
                    var myOption = configurationRoot.Get<MyOption>();
                    Console.WriteLine($"EndpointId: {myOption?.EndpointId}");
                    Console.WriteLine($"DisplayLabel: {myOption?.DisplayLabel}");
                    Console.WriteLine($"WidgetRoute: {myOption?.WidgetRoute}");


                });
        // Sample output:
        //    WidgetRoute=api/widgets
        //    EndpointId=b3da3c4c-9c4e-4411-bc4d-609e2dcc5c67
        //    DisplayLabel=Widgets Incorporated, LLC.


    }
}
