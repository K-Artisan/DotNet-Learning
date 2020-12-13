using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Di.Sample1
{
    public class Worker : BackgroundService
    {
        private readonly IMessageWriter _messageWriter;

        public Worker(IMessageWriter messageWriter)
        {
            _messageWriter = messageWriter;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _messageWriter.Write($"Woker running at { DateTime.UtcNow}");
                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
