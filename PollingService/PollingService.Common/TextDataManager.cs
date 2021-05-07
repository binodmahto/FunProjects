using System;
using System.IO;

namespace PollingService.Common
{
    public class TextDataManager : IPullerServiceDataManager
    {
        private readonly string _filePath = Path.Combine(Environment.CurrentDirectory, "Data.txt");

        public DateTime GetLastPullDateTime()
        {
            lock (_filePath)
            {
                string data = File.ReadAllText(_filePath);
                if (DateTime.TryParse(data, out var dateTime))
                    return dateTime;
                else
                    return DateTime.MinValue;
            }
        }

        public void UpdateLastPullDateTime(DateTime lastPulledDateTime)
        {
            lock (_filePath)
            {
                File.WriteAllText(_filePath, lastPulledDateTime.ToString());
            }
        }
    }
}
