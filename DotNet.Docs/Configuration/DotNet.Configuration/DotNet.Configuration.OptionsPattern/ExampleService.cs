using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace DotNet.Configuration.OptionsPattern
{
    public class ExampleService
    {
        public string Id { get; set; }
        private readonly IServiceProvider _serviceProvider;

        public ExampleService(IServiceProvider serviceProvider)
        {
            Id = Guid.NewGuid().ToString()[^4..];
            _serviceProvider = serviceProvider;
        }

        public void DisplayValues()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var option = scope.ServiceProvider.GetRequiredService<IOptions<MyOption>>().Value;
                Console.WriteLine($"{Id}.{nameof(ExampleService)}.IOptions<MyOption>: {Newtonsoft.Json.JsonConvert.SerializeObject(option)}");
                
                var optionsSnapshot = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<MyOption>>().Value;
                Console.WriteLine($"{Id}.{nameof(ExampleService)}.IOptionsSnapshot<MyOption>-1: {Newtonsoft.Json.JsonConvert.SerializeObject(optionsSnapshot)}");
                var optionsSnapshot2 = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<MyOption>>().Value;
                Console.WriteLine($"{Id}.{nameof(ExampleService)}.IOptionsSnapshot<MyOption>-2: {Newtonsoft.Json.JsonConvert.SerializeObject(optionsSnapshot2)}");

                var optionsMonitor = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<MyOption>>().CurrentValue;
                Console.WriteLine($"{Id}.{nameof(ExampleService)}.IOptionsMonitor<MyOption>: {Newtonsoft.Json.JsonConvert.SerializeObject(optionsMonitor)}");

                Console.WriteLine();

            }
        }

    }

}
