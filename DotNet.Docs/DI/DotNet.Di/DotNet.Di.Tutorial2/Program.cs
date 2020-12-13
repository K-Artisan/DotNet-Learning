using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DotNet.Di.Tutorial2
{
    /// <summary>
    /// 文档地址：
    /// https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines
    /// 本示例展示：
    ///     DI的最佳实践:
    /// 1. 从容器中解析的对象，开发者不要自己手动释放，容器会自己管理
    ///    
    ///    https://www.cnblogs.com/artech/p/async-di.html
    /// </summary>
    class Program
    {
        static Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            ExemplifyDisposableScoping(host.Services, "Scope 1");
            Console.WriteLine();

            ExemplifyDisposableScoping(host.Services, "Scope 2");
            Console.WriteLine();

            return host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddTransient<TransientDisposable>()
                            .AddScoped<ScopedDisposable>()
                            .AddSingleton<SingletonDisposable>());

        static void ExemplifyDisposableScoping(IServiceProvider services, string scope)
        {
            Console.WriteLine($"{scope}...");

            using IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            _ = provider.GetRequiredService<TransientDisposable>();
            _ = provider.GetRequiredService<ScopedDisposable>();
            _ = provider.GetRequiredService<SingletonDisposable>();
        }
    }
}
