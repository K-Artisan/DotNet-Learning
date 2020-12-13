using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DotNet.Di.Tutorial3
{
    /*
    # 本示例展示：DI的最佳实践:

    ## 1. 如何正确的解决Shared instance, limited lifetime的问题,
        Shared instance, limited lifetime
        Scenario
            The app requires a shared IDisposable instance across multiple services,
            but the IDisposable instance should have a limited lifetime.
        Solution
           Register the instance with a scoped lifetime.
           Use IServiceScopeFactory.CreateScope to create a new IServiceScope.
           Use the this new scope's IServiceProvider to get required services.
           Dispose the scope when it's no longer needed.

           典型的场景：
           https://www.cnblogs.com/artech/p/async-di.html
    */
    class Program
    {
        static Task Main(string[] args)
        {
            return CreateHostBuilder(args).Build().RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services
                         //.AddHostedService<Worker>()  //ok
                         //.AddHostedService<WorkerError>()  //不再循环输出
                        .AddHostedService<WorkerErrorToOk>()  //修复，能循环输出
                        .AddScoped<IScopedOperation, ScopedOperation>();

            });
    }
}
