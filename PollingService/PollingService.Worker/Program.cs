using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PollingService.Common;
using PollingService.Puller;
using PollingService.Queue;
using Serilog;

namespace PollingService.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    IConfigProvider config = new DefaultConfigurationProvider()
                    {
                        MaxAllowedConcurrentFilePulling = Convert.ToInt32(configuration.GetSection("appSettings")["MaxAllowedConcurrentFilePulling"]),
                        PullEverySeconds = Convert.ToInt32(configuration.GetSection("appSettings")["PullEverySeconds"]),
                        Source = configuration.GetSection("appSettings")["SourceFolder"],
                        Destination = configuration.GetSection("appSettings")["DestFolder"]
                    };
                    services.AddSingleton(config);

                    services.AddSingleton<IPullerServiceDataManager, TextDataManager>();

                    services.AddSingleton<IPollingQueue, InHouseQueue>();

                    services.AddSingleton<IFilePuller, LocalFilePuller>();

                    services.AddHostedService<Worker>();
                }).ConfigureLogging(loggingBuilder =>
                {
                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();
                    var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();
                    loggingBuilder.AddSerilog(logger, dispose: true);
                });
    }
}
