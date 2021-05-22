using Demo.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace DotNetCoreLogging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Logger.Info("Starting Web Host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Host terminated unexpectedly");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
