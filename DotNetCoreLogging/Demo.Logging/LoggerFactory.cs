using Demo.Logging.Model;
using Demo.Logging.Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Logging
{
    public class LoggerFactory
    {
        private static volatile Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();
        private readonly static object _sync = new object();
      //  private static IConfiguration _configuration;

        //public LoggerFactory(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        public static ILogger GetLogger()
        {
            var controllerName ="General";
            if (!_loggers.TryGetValue(controllerName, out ILogger logger))
            {
                lock (_sync)
                {
                    if (!_loggers.TryGetValue(controllerName, out logger))
                    {
                        logger = new SerilogFileLogger();
                        _loggers.Add(controllerName, logger);
                    }
                }
            }

            return logger;
        }
        public static ILogger GetLogger<T>()
        {
            var controllerName = typeof(T).Name;
            if (!_loggers.TryGetValue(controllerName, out ILogger logger))
            {
                lock (_sync)
                {
                    if (!_loggers.TryGetValue(controllerName, out logger))
                    {
                        // if(_configuration.GetSection("Serilog").Exists())
                        var logFilePath = $"C:\\Temp\\{controllerName}.log";
                        logger = new SerilogFileLogger(logFilePath, LogLevels.All);
                        _loggers.Add(controllerName, logger);
                    }
                }
            }

            return logger;
        }
    }
}
