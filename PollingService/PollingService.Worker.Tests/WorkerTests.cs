using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PollingService.Common;
using PollingService.Puller;
using PollingService.Queue;
using PollingService.Worker;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace PollingService.Worker.Tests
{
    [TestClass]
    public class WorkerTests
    {
        [TestMethod]
        public async Task ExecuteAsync_Test()
        {
            try
            {
                IServiceCollection services = new ServiceCollection();
                services.AddSingleton(Mock.Of<IPullerServiceDataManager>());
                services.AddSingleton(Mock.Of<IPollingQueue>());
                services.AddSingleton(Mock.Of<IFilePuller>());

                IConfigProvider config = new DefaultConfigurationProvider()
                {
                    MaxAllowedConcurrentFilePulling = 1,
                    PullEverySeconds = 10,
                    Source = "C:\\temp",
                    Destination = "C:\\temp2"
                };
                services.AddSingleton<IConfigProvider>(config);
                ILogger<Worker> logger = new NullLogger<Worker>();
                services.AddSingleton(logger);
                services.AddHostedService<PollingService.Worker.Worker>();

                var serviceProvider = services.BuildServiceProvider();
                var backgroundService = serviceProvider.GetService<IHostedService>() as Worker;

                await backgroundService?.StartAsync(CancellationToken.None);
                await Task.Delay(1000);

                await backgroundService?.StopAsync(CancellationToken.None);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
