using System;

namespace PollingService.Common
{
    public interface IPullerServiceDataManager
    {
        DateTime GetLastPullDateTime();

        void UpdateLastPullDateTime(DateTime dateTime);
    }
}
