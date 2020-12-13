using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Di.Tutorial3
{
    public class WorkerErrorToOk : BackgroundService
    {
        private IServiceProvider _serviceProvider;
        private IServiceScopeFactory _serviceScopeFactory;

        public WorkerErrorToOk(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceProvider = serviceProvider;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);

                    var scopedDisposable = scope.ServiceProvider.GetRequiredService<IScopedOperation>();
                    scopedDisposable.Display();

                    _serviceProvider = null; //能模拟根_serviceProvider 清空吗？
                }
            }
        }
    }
}
