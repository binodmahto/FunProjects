using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PollingService.Common;
using PollingService.Puller;
using PollingService.Queue;

namespace PollingService.Worker
{
    public class Worker : BackgroundService
    {
        private System.Timers.Timer _timer;
        private readonly ILogger<Worker> _logger;
        private readonly IConfigProvider _config;
        private readonly IPollingQueue _pollingQueue;
        private readonly IFilePuller _puller;
        private readonly IPullerServiceDataManager _dataManager;
        private readonly object _lock = new object();

        public Worker(ILogger<Worker> logger, IConfigProvider config, IPollingQueue pollingQueue, IFilePuller puller, IPullerServiceDataManager dataManager)
        {
            _logger = logger;
            _config = config;
            _pollingQueue = pollingQueue;
            _puller = puller;
            _dataManager = dataManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Polling Service has started successfully at:{DateTime.Now}");
            TimeSpan interval = new TimeSpan(0, 0, 0, _config.PullEverySeconds);
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _timer = new System.Timers.Timer { Interval = interval.TotalMilliseconds };
                _timer.Elapsed += Timer_Elapsed;
                _timer.Enabled = true;
                _timer.AutoReset = true;
                _timer.Start();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    var directory = new DirectoryInfo(_config.Source);
                    var files = directory.GetFiles()
                        .Where(file => file.CreationTime >= _dataManager.GetLastPullDateTime());
                    if (files.Any())
                    {
                        foreach (var fileInfo in files)
                        {
                            _pollingQueue.QueueFileTransferTask(ct =>
                                _puller.MoveFile(new FileTransferInfo(fileInfo.Name, _config.Source, _config.Destination)));
                        }
                    }
                    _logger.LogInformation($"Polling has completed successfully at:{DateTime.Now}");
                }
                catch (Exception exception)
                {
                    _logger.LogError($"Polling failed at:{DateTime.Now} with exception:{exception}");
                }
            }
        }
    }
}
