using System;

namespace Demo.Logging.Model
{
    /// <summary>
    /// An interface for logging
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes the given message if logging is turned on at the given level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        void Write(object message, LogLevels level)
        {
            Write(message, level, string.Empty);
        }


        /// <summary>
        /// Writes the given message if logging is turned on at the given level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="categoryName"></param>
        void Write(object message, LogLevels level, string categoryName);

        /// <summary>
        /// Writes the value returned by the message delegate, if logging is turned on at the given level for the given category
        /// </summary>
        /// <param name="msgDelegate"></param>
        /// <param name="level"></param>
        void Write(Func<object> msgDelegate, LogLevels level)
        {
            Write(msgDelegate, level, string.Empty);
        }

        /// <summary>
        /// Writes the value returned by the message delegate, if logging is turned on at the given level for the given category
        /// </summary>
        /// <param name="msgDelegate"></param>
        /// <param name="level"></param>
        /// <param name="categoryName"></param>
        void Write(Func<object> msgDelegate, LogLevels level , string categoryName);

        /// <summary>
        /// Tells if logging is enabled for the <see cref="LogLevels"/> or not.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        bool ShouldLog(LogLevels level);
    }

    /// <summary>
    /// Valid Log levels for Prophet 21. More detailed logging may introduce significant performance costs.
    /// </summary>
    public enum LogLevels : byte
    {
        /// <summary>
        /// No logging
        /// </summary>
        None, 
        /// <summary>
        /// Error logs only
        /// </summary>
        Error,
        /// <summary>
        /// All warnings and errors
        /// </summary>
        Warning,
        /// <summary>
        /// All informational logging, warnings and errors
        /// </summary>
        Information,
        /// <summary>
        /// Detailed debug level logging, information, warning and errors
        /// </summary>
        Debug,
        /// <summary>
        /// Trace level logging, debug messages, informational messages, warnings and errors
        /// </summary>
        Trace,
        /// <summary>
        ///  All logging including nested trace messages.
        /// </summary>
        All
    }
}