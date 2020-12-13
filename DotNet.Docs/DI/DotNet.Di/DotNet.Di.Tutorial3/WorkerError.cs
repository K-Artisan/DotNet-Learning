using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Di.Tutorial3
{
    public class WorkerError : BackgroundService
    {
        private IServiceProvider _serviceProvider;

        public WorkerError(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);

                var scopedOperation = _serviceProvider.GetRequiredService<IScopedOperation>();
                scopedOperation.Display();

                _serviceProvider = null;
            }
        }
    }
}
