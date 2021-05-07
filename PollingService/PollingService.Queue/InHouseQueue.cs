using Microsoft.Extensions.Logging;
using PollingService.Common;
using System;
using System.Threading;

namespace PollingService.Queue
{
    public class InHouseQueue : IPollingQueue, IDisposable
    {
        private readonly BlockingMethodQueue _fileTransferQueue;
        private readonly ILogger<InHouseQueue> _logger;

        public InHouseQueue(IConfigProvider configProvider, ILogger<InHouseQueue> logger)
        {
            _logger = logger;
            //Fire up a blocking method queue with the number of threads we have specified
            _fileTransferQueue = new BlockingMethodQueue(logger, configProvider.MaxAllowedConcurrentFilePulling);
        }

        public void Dispose()
        {
            _fileTransferQueue.Dispose();
        }

        public void QueueFileTransferTask(Action<CancellationToken> workItem)
        {
            _fileTransferQueue.Add(() => workItem.Invoke(new CancellationToken()));
        }
    }
}
