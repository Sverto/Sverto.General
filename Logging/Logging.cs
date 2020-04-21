using System;

namespace Sverto.General.Logging
{
    /// <summary>
    /// Global logger that can ben used to log from anywhere
    /// </summary>
    public static class Logging
    {

        /// <summary>
        /// Method called when a new log message arives
        /// </summary>
        /// <returns></returns>
        public static Logger GlobalLogger { get; } = new Logger();

        /// <summary>
        /// Alias for GlobalLogger.Log(...)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        public static void Log(object sender, LogLevel logLevel, string message)
        {
            lock (GlobalLogger)
            {
                GlobalLogger.Log(sender, logLevel, message);
            }
        }

        /// <summary>
        /// Alias for GlobalLogger.Log(...)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public static void Log(object sender, LogLevel logLevel, string message, Exception innerException)
        {
            lock (GlobalLogger)
            {
                GlobalLogger.Log(sender, logLevel, message, innerException);
            }
        }

    }
}
