using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Di.Tutorial3
{
    public class Worker : BackgroundService
    {
        private readonly IScopedOperation _scopedOperation;

        public Worker(IScopedOperation scopedDisposable)
        {
            _scopedOperation = scopedDisposable;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                _scopedOperation.Display();
            }

        }
    }
}
