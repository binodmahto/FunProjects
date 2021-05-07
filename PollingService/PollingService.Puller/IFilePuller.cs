using PollingService.Common;

namespace PollingService.Puller
{
    public interface IFilePuller
    {
        void MoveFile(FileTransferInfo fileInfo);
    }
}
