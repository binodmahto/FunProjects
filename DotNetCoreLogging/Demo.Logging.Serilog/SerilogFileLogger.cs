using System;
using System.IO;
using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Events;
using Demo.Logging.Model;
using Serilog.Formatting.Compact;

namespace Demo.Logging.Serilog
{
    public class SerilogFileLogger : Model.ILogger
    {
        /// <summary>
        /// Default constructor reading settings from config (appsettings.json) file.
        /// </summary>
        public SerilogFileLogger()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logFilePath">Fully qualified log file path i.e. c:\temp\log.json</param>
        /// <param name="level"><see cref="LogLevels"/> for the log</param>
        /// <param name="fileSizeLimitBytes">Max log file size</param>
        public SerilogFileLogger(string logFilePath, LogLevels level, long? fileSizeLimitBytes = null)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .WriteTo.File(new CompactJsonFormatter(), logFilePath, fileSizeLimitBytes: fileSizeLimitBytes ?? 1073741824,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1))
               .CreateLogger();
        }

        public void Write(object message, LogLevels level)
        {
            WriteLog(message, level);
        }

        public void Write(object message, LogLevels level, string category)
        {
            WriteLog(message, level, category);
        }

        public void Write(Func<object> msgDelegate, LogLevels level, string category)
        {
            WriteLog(msgDelegate.Invoke(), level, category);
        }

        public bool ShouldLog(LogLevels level)
        {
            return Log.IsEnabled(ToLogEventLevel(level));
        }

        private void WriteLog(object message, LogLevels level, string category = "")
        {
            var ex = message as Exception;
            var messageTemplate = $"{message}";
            if (!String.IsNullOrEmpty(category))
                messageTemplate =  $"{category}=>{(ex == null ? ex.Message : message)}";
            
            switch (level)
            {
                case LogLevels.Error:
                    if (ex != null)
                        Log.Error(ex, messageTemplate);
                    else
                        Log.Error(messageTemplate);
                    break;
                case LogLevels.Warning:
                    if (ex != null)
                        Log.Warning(ex, messageTemplate);
                    else
                        Log.Warning(messageTemplate);
                    break;
                case LogLevels.Information:
                    if (ex != null)
                        Log.Information(ex, messageTemplate);
                    else
                        Log.Information(messageTemplate);
                    break;
                case LogLevels.Debug:
                case LogLevels.Trace:
                case LogLevels.All:
                    if (ex != null)
                        Log.Verbose(ex, messageTemplate);
                    else
                        Log.Verbose(messageTemplate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            
        }

        private LogEventLevel ToLogEventLevel(LogLevels level)
        {
            switch (level)
            {
                case LogLevels.Error:
                    return LogEventLevel.Error;
                case LogLevels.Warning:
                    return LogEventLevel.Warning;
                case LogLevels.Information:
                    return LogEventLevel.Information;
                case LogLevels.Debug:
                case LogLevels.Trace:
                case LogLevels.All:
                    return LogEventLevel.Verbose;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}
