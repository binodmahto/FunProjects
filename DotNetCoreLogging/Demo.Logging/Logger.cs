using Demo.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Logging
{
    /// <summary>
    /// Static class used to route messages to the appropriate Log Implementation
    /// </summary>
    public class Logger
    {

        ///// <summary>
        ///// The <see cref="ILogger" /> implementation used by the static Logger
        ///// </summary>
        //public ILogger LogImplementation { get; set; } = LoggerFactory.GetLogger();

        /// <summary>
        /// Write the specified message, if the logging implementation is configured at the specified level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public static void Write(object message, LogLevels level)
        {
            LoggerFactory.GetLogger().Write(message, level);
        }

        /// <summary>
        /// Write the specified message, if the logging implementation is configured at the specified level and category
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="category"></param>
        public static void Write(object message, LogLevels level, string category)
        {
            LoggerFactory.GetLogger().Write(message, level, category);
        }

        /// <summary>
        /// Write the result of the given message delegate, if the logging
        /// implementation is configured at the specified level/category.
        /// This method can allow the caller to defer construction of
        /// complex or costly message until after evaluating configuration.
        /// </summary>
        /// <param name="msgDelegate"></param>
        /// <param name="level"></param>
        /// <param name="category"></param>
        public static void Write(Func<object> msgDelegate, LogLevels level, string category = null)
        {
            LoggerFactory.GetLogger().Write(msgDelegate, level, category);
        }

        /// <summary>
        /// Is the loggimg implementation configured at the given level/category
        /// </summary>
        /// <param name="level"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static bool ShouldLog(LogLevels level, string category = null)
        {
            return LoggerFactory.GetLogger().ShouldLog(level);
        }

        /// <summary>
        /// Write a message at the Error level
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message) => Error(message, null);
        /// <summary>
        /// Write a message at the specified category to the Error level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        public static void Error(object message, string category) => Write(message, LogLevels.Error, category);

        /// <summary>
        /// Write a message at the Information level
        /// </summary>
        /// <param name="message"></param>
        public static void Info(object message) => Info(message, null);
        /// <summary>
        /// Write a message at the specified category to the Information level
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// </summary>
        public static void Info(object message, string category) => Write(message, LogLevels.Information, category);

        /// <summary>
        /// Write a message at the Warning level
        /// </summary>
        /// <param name="message"></param>
        public static void Warning(object message) => Warning(message, null);
        /// <summary>
        /// Write a message at the specified category to the Warning level
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// </summary>
        public static void Warning(object message, string category) => Write(message, LogLevels.Warning, category);

        /// <summary>
        /// Write a message at the Debug level
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(object message) => Debug(message, null);
        /// <summary>
        /// Write a message at the specified category to the Debug level
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// </summary>
        public static void Debug(object message, string category) => Write(message, LogLevels.Debug, category);

        /// <summary>
        /// Write a message at the Trace level
        /// <param name="message"></param>
        /// </summary>
        public static void Trace(object message) => Trace(message, null);
        /// <summary>
        /// Write a message at the specified category to the Trace level
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// </summary>
        public static void Trace(object message, string category) => Write(message, LogLevels.Trace, category);
    }

    /// <summary>
    /// Static class used to route messages to the appropriate Log Implementation with a new logfile file named with <see cref="typeof(T).Name"/> 
    /// </summary>
    public static class Logger<T>
    {

        //static Logger()
        //{
        //    Logger.LogImplementation = LoggerFactory.GetLogger<T>();
        //}
        ///// <summary>
        ///// The <see cref="ILogger" /> implementation used by the static Logger
        ///// </summary>
        //public static ILogger LogImplementation => LoggerFactory.CreateLogger<T>();

        /// <summary>
        /// Write the specified message, if the logging implementation is configured at the specified level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public static void Write(object message, LogLevels level) => LoggerFactory.GetLogger<T>().Write(message, level);

        /// <summary>
        /// Write the specified message, if the logging implementation is configured at the specified level and category
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="category"></param>
        public static void Write(object message, LogLevels level, string category) => LoggerFactory.GetLogger<T>().Write(message, level, category);

        /// <summary>
        /// Is the loggimg implementation configured at the given level/category
        /// </summary>
        /// <param name="level"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static bool ShouldLog(LogLevels level, string category = null)
        {
            return LoggerFactory.GetLogger<T>().ShouldLog(level);
        }

        /// <summary>
        /// Write a message at the Error level
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message) => Error(message, null);
        /// <summary>
        /// Write a message at the specified category to the Error level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        public static void Error(object message, string category) => Write(message, LogLevels.Error, category);

        /// <summary>
        /// Write a message at the Information level
        /// </summary>
        /// <param name="message"></param>
        public static void Info(object message) => Info(message, null);
        /// <summary>
        /// Write a message at the specified category to the Information level
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// </summary>
        public static void Info(object message, string category) => Write(message, LogLevels.Information, category);

        /// <summary>
        /// Write a message at the Warning level
        /// </summary>
        /// <param name="message"></param>
        public static void Warning(object message) => Warning(message, null);
        /// <summary>
        /// Write a message at the specified category to the Warning level
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// </summary>
        public static void Warning(object message, string category) => Write(message, LogLevels.Warning, category);

        /// <summary>
        /// Write a message at the Debug level
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(object message) => Debug(message, null);
        /// <summary>
        /// Write a message at the specified category to the Debug level
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// </summary>
        public static void Debug(object message, string category) => Write(message, LogLevels.Debug, category);

        /// <summary>
        /// Write a message at the Trace level
        /// <param name="message"></param>
        /// </summary>
        public static void Trace(object message) => Trace(message, null);
        /// <summary>
        /// Write a message at the specified category to the Trace level
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// </summary>
        public static void Trace(object message, string category) => Write(message, LogLevels.Trace, category);
    }
}

