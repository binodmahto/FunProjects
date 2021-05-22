using System;

namespace Demo.Logging.Model
{
    public class NullLogger : ILogger
    {
        public bool ShouldLog(LogLevels level)
        {
            return false;
        }

        public void Write(object message, LogLevels level, string categoryName)
        {
            //Do Nothing
        }

        public void Write(Func<object> msgDelegate, LogLevels level, string categoryName)
        {
            //Do Nothing
        }
    }
}
