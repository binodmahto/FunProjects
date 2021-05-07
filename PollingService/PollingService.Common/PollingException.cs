using System;
using System.Collections.Generic;
using System.Text;

namespace PollingService.Common
{
    public class PollingException : Exception
    {
        public PollingException(string msg) : base(msg)
        {
        }

        public PollingException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
