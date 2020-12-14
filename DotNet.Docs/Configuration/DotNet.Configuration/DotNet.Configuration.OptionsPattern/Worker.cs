using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DotNet.Configuration.OptionsPattern
{
    public class Worker : BackgroundService
    {
        private readonly ExampleService _exampleService;
        private readonly ScopedService _scopedService;
        private readonly MonitorService _monitorService;

        public Worker(ExampleService exampleService,
            ScopedService scopedService,
            MonitorService monitorService)
        {
            _exampleService = exampleService;
            _scopedService = scopedService;
            _monitorService = monitorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"\n\r--------------at { DateTime.UtcNow}--------------");

                _exampleService.DisplayValues();
                _scopedService.DisplayValues();
                _monitorService.DisplayValues();

                await Task.Delay(5 * 1000, stoppingToken);
            }
        }
    }
}
