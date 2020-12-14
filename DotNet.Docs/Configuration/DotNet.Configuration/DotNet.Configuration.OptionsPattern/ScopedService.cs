using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Configuration.OptionsPattern
{
    public class ScopedService
    {
        public string Id { get; set; }
        private readonly IServiceProvider _serviceProvider;

        public ScopedService(IServiceProvider serviceProvider)
        {
            Id = Guid.NewGuid().ToString()[^4..];
            _serviceProvider = serviceProvider;
        }

        public void DisplayValues()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var optionsSnapshot = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<MyOption>>().Value;
                Console.WriteLine($"{Id}.{nameof(ScopedService)}.IOptionsSnapshot<MyOption>: {Newtonsoft.Json.JsonConvert.SerializeObject(optionsSnapshot)}");
            }
        }

    }

}
