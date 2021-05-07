using PollingService.Common;
using System;
using System.IO;

namespace PollingService.Puller
{
    public class LocalFilePuller : IFilePuller
    {
        private readonly IPullerServiceDataManager _dataManager;
        public LocalFilePuller(IPullerServiceDataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public void MoveFile(FileTransferInfo fileInfo)
        {
            var source = Path.Combine(fileInfo.Source, fileInfo.FileName);
            var dest = Path.Combine(fileInfo.Destination, fileInfo.FileName);
            lock (dest)
            {
                File.Move(source, dest);
                _dataManager.UpdateLastPullDateTime(DateTime.Now);
            }
        }
    }
}
