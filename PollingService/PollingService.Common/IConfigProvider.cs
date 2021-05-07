using System;
using System.Collections.Generic;
using System.Text;

namespace PollingService.Common
{
    public interface IConfigProvider
    {
        int MaxAllowedConcurrentFilePulling { get; set; }

        int PullEverySeconds { get; set; }

        string Source { get; set; }

        string Destination { get; set; }
    }

    public class DefaultConfigurationProvider : IConfigProvider
    {
        public int MaxAllowedConcurrentFilePulling { get; set; }
        public int PullEverySeconds { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

    }
}
