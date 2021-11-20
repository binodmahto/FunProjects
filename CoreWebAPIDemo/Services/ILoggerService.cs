
namespace CoreWebAPIDemo.Services
{
    public enum LogType
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error
    }

    public interface ILoggerService
    {
        public void Log(LogType logType, object logInfo);
    }

    public class NullLoggerService : ILoggerService
    {
        public void Log(LogType logType, object logInfo)
        {
            //Do Nothing
        }
    }

    public class SerilogFileLoggerService : ILoggerService
    {
        public void Log(LogType logType, object logInfo)
        {
            //your logging code
        }
    }

    public class DbLoggerService : ILoggerService
    {
        public void Log(LogType logType, object logInfo)
        {
            //your logging code
        }
    }
}
