using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DotNet.Configuration.Overview2
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration
    /// </summary>
    class Program
    {
        static Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            ExemplifyConfiguration(host.Services);

            return host.RunAsync();

        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    //configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("myoption.json", optional: false); //appsettings.json 存在同名节点，优先取appsettings.json中的配置
                    //configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                    //configHost.AddCommandLine(args);
                });
        }

        static void ExemplifyConfiguration(IServiceProvider services)
        {
            using IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            //依赖注入中获取IConfiguration
            var configuration = provider.GetRequiredService<IConfiguration>();

            //自己创建获取IConfiguration
            //IConfigurationRoot configurationRoot = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", false)
            //    .Build();

            //---------------------获取配置值的方法--------------------------------
            //GetSection(xxx:x1)
            var myOption1 = configuration.GetSection("MyOption:Option1")?.Value;
            Console.WriteLine($"Option1: {myOption1}");

            //GetSection(xxx)[x1]
            var myOption2 = configuration.GetSection("MyOption")?["Option2"];
            Console.WriteLine($"Option2: {myOption2}");

            //创建对象
            var myOption = configuration.GetSection("MyOption").Get<MyOption>();
            Console.WriteLine($"Option1: {myOption?.Option1}");
            Console.WriteLine($"Option2: {myOption?.Option2}");
        }
    }
}
