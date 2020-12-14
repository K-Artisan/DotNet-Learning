using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Configuration.OptionsPattern
{
    public class MonitorService
    {
        public string Id { get; set; }
        private readonly IServiceProvider _serviceProvider;

        public MonitorService(IServiceProvider serviceProvider)
        {
            Id = Guid.NewGuid().ToString()[^4..];
            _serviceProvider = serviceProvider;
        }

        public void DisplayValues()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var optionsMonitor = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<MyOption>>().CurrentValue;
                Console.WriteLine($"{Id}.{nameof(MonitorService)}.IOptionsMonitor<MyOption>: {Newtonsoft.Json.JsonConvert.SerializeObject(optionsMonitor)}");
            }
        }

    }

}
