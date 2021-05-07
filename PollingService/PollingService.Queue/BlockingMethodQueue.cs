using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace PollingService.Queue
{
    /// <summary>
    /// Provides a thread pool for method execution.
    /// </summary>
    public class BlockingMethodQueue : IDisposable
    {
        private readonly BlockingCollection<Action> _actionQueue;
        private readonly int _threadCount;
        private bool _disposing;
        private readonly ILogger<InHouseQueue> _logger;


        /// <summary>
        /// Initializes a new instance of the BlockingMethodQueue with a thread pool
        /// of the given size.
        /// </summary>
        /// <param name="threadCount"></param>
        public BlockingMethodQueue(ILogger<InHouseQueue> logger, int threadCount = 1)
        {
            _logger = logger;
            if (threadCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(threadCount),
                    "Blocking method queue thread count must be greater than zero (0).");
            }

            _actionQueue = new BlockingCollection<Action>(new ConcurrentQueue<Action>());
            _threadCount = threadCount;
            Start();
        }

        private bool IsDisposing => _disposing;

        private void Start()
        {
            for (var i = 0; i < _threadCount; i++)
            {
                var threadNo = i;
                var traceString = $"MethodQueue - Thread Number { threadNo } Process Id { Process.GetCurrentProcess().Id.ToString() } Thread Id { Thread.CurrentThread.ManagedThreadId }: ";
               _logger.LogTrace(traceString + "Initiate Subscriber Thread", "Polling Service Queue");

                System.Threading.Tasks.Task.Run(() =>
                {
                    RunWorkerThread(threadNo);
                });
            }
        }

        private void RunWorkerThread(int threadNo)
        {
            while (!IsDisposing)
            {
                // Update traceString so thread updated
                var traceString = 
                    $"MethodQueue - Thread Number {threadNo} Process Id {Process.GetCurrentProcess().Id.ToString()} Thread Id {Thread.CurrentThread.ManagedThreadId}: ";

                Action method;

                try
                {
                    _logger.LogTrace(traceString + "Ready for work", "Polling Service Queue");
                    //This method blocks until there is an item available
                    method = _actionQueue.Take();
                    _logger.LogTrace(traceString + "Takes from Queue", "Polling Service Queue");
                }
                catch (Exception takeEx)
                    when (takeEx is ObjectDisposedException || takeEx is InvalidOperationException)
                {
                    //The queue is shutting down. Log the removal of the thread from our queue.
                    _logger.LogWarning($"Thread {threadNo} will be removed from service. Exception:{takeEx}");
                    return;
                }
                catch (Exception ex)
                {
                    //Something else happened. Do not abandon this thread.
                    _logger.LogWarning($"Thread {threadNo} will wait for the next available method.Exception:{ex}");
                    continue;
                }

                try
                {
                    _logger.LogTrace(traceString + "Invoke starts", "Polling Service Queue");
                    method?.Invoke();
                }
                catch (Exception e)
                {
                    _logger.LogError($"Pulling service failed to perform with an exception:{e}");
                }
                finally
                {
                  _logger.LogTrace(traceString + "Invoke ends", "Polling Service Queue");
                }
            }
        }

        public void Add(Action a)
        {
            _actionQueue.Add(a);
        }


        public void Dispose()
        {
            _disposing = true;
            _actionQueue?.Dispose();
        }
    }
}