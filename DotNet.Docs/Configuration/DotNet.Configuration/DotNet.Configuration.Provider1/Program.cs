using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DotNet.Configuration.Provider1
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
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();

                    IHostEnvironment env = hostingContext.HostingEnvironment;

                    configuration
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) //using JSON configuration provider
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                        .AddXmlFile("appsettings.xml", optional: true, reloadOnChange: true); //using XML configuration provider

                    IConfigurationRoot configurationRoot = configuration.Build();

                    //--------Json Configuration Provider--------
                    var optionsJson = new MyOption();
                    configurationRoot.GetSection(nameof(MyOption))
                                     .Bind(optionsJson);

                    Console.WriteLine("--------Json Configuration Provider--------");
                    Console.WriteLine($"MyOption->Option1: {optionsJson?.Option1}");
                    Console.WriteLine($"MyOption->Option2: {optionsJson?.Option2}");


                    //--------Xml Configuration Provider--------
                    var optionsXml = new TransientFaultHandlingOptions();
                    configurationRoot.GetSection(nameof(TransientFaultHandlingOptions))
                                     .Bind(optionsXml);
                    Console.WriteLine("--------Xml Configuration Provider--------");
                    Console.WriteLine($"TransientFaultHandlingOptions->Option1: {optionsXml?.Enabled}");
                    Console.WriteLine($"TransientFaultHandlingOptions->Option2: {optionsXml?.AutoRetryDelay}");

                });
    }
}
