
    # 文档地址：
    https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines
  
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

       两个进程，IFoobar（要求被注入短生命周期）是在main进程中实例化，但是随着mian进程完成而被释放了，
       所以内部进程要有自己的scope，而这个可有`IServiceScopeFactory.CreateScope`来创建


       `main.cs`
       ```c#
               Host
            .CreateDefaultBuilder()
            .UseDefaultServiceProvider(options => options.ValidateScopes = true)
            .ConfigureWebHostDefaults(builder => builder
                .ConfigureLogging(logging => logging.ClearProviders())
                .ConfigureServices(services => services
                    .AddScoped<IFoobar, Foobar>()
                    .AddRouting()
                    .AddControllers())
                .Configure(app => app
                    .UseRouting()
                    .UseEndpoints(endpoints => endpoints.MapControllers())))
            .Build()
            .Run();
       ```


       `HomeController.cs`
       ```C#
public class HomeController : Controller
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HomeController(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        Task.Run(async () =>
        {
            await Task.Delay(100);
            using (var scope = _serviceScopeFactory.CreateScope())
             { 
                var foobar = scope.ServiceProvider.GetRequiredService<IFoobar>();
             }
        });
        return Ok();
    }
}

       ```

  
      ## [服务实例的生命周期](https://www.cnblogs.com/artech/p/inside-asp-net-core-03-08.html)
 
       