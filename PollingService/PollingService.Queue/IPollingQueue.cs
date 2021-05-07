using System;
using System.Threading;

namespace PollingService.Queue
{
    public interface IPollingQueue
    {
        void QueueFileTransferTask(Action<CancellationToken> workItem);
    }

}
