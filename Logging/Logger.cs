using System;
using System.Windows.Forms;

namespace Sverto.General.Logging
{
    /// <summary>
	/// A logger with loglevel and the option to directly write to a file.
	///  Usage:
	///   Log(...) or GlobalLogger.Log(...)
	///   Set delegate: GlobalLogger.LogMethod
	/// With delegate support.
	/// </summary>
	/// <remarks></remarks>
	public class Logger
    {

        internal Logger()
        {
        }

        #region "Fields & Properties"
        private readonly object _ThreadSafeProperty = new object();
        private readonly object _ThreadSafeFileWrite = new object();
        private readonly object _ThreadSafeClient = new object();

        public const string DateFormatTag = "{d}";
        public const string LogLevelFormatTag = "{l}";
        public const string SourceFormatTag = "{s}";
        public const string MessageFormatTag = "{m}";
        public string MessageFormat { get; set; } = DateFormatTag + " <" + LogLevelFormatTag + "/" + SourceFormatTag + "> " + MessageFormatTag;

        public delegate void LoggerDelegate(LogMessage logMessage, string formattedMessage);
        protected LoggerDelegate _LogMethod;
        public LoggerDelegate LogMethod
        {
            get
            {
                lock (_ThreadSafeProperty)
                {
                    return _LogMethod;
                }
            }
            set
            {
                lock (_ThreadSafeProperty)
                {
                    _LogMethod = value;
                }
            }
        }

        private string _FilePath = "";
        public string FilePath
        {
            get
            {
                lock (_ThreadSafeProperty)
                {
                    return _FilePath;
                }
            }
            set
            {
                lock (_ThreadSafeProperty)
                {
                    _FilePath = value;
                }
            }
        }

        private LogLevel _FileLogLevels = (LogLevel.Info | LogLevel.Warning | LogLevel.Error);
        public LogLevel FileLogLevels
        {
            get
            {
                lock (_ThreadSafeProperty)
                {
                    return _FileLogLevels;
                }
            }
            set
            {
                lock (_ThreadSafeProperty)
                {
                    _FileLogLevels = value;
                }
            }
        }

        public object ClientLock
        {
            get { return _ThreadSafeClient; }
        }
        #endregion

        #region "Events"
        public event NewLogMessageEventHandler NewLogMessage;
        public delegate void NewLogMessageEventHandler(object sender, LoggerEventArgs e);

        protected void OnNewLogMessage(object sender, LoggerEventArgs e)
        {
            NewLogMessage?.Invoke(sender, e);
        }
        #endregion

        #region "Methods"
        /// <summary>
        /// Log a message to a file (if FilePath is set) and execute the user's delegate (if set) with LogMessage info.
        /// </summary>
        /// <param name="sender">This can be the Object or a String containing the name of the Object</param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        public void Log(object sender, LogLevel logLevel, string message)
        {
            Log(new LogMessage(sender, DateTime.Now, logLevel, message));
        }

        public void Log(object sender, LogLevel logLevel, string message, Exception innerException)
        {
            Log(new LogMessage(sender, DateTime.Now, logLevel, message + "\r\nInner Exception: " + innerException.ToString()));
        }

        public void Log(object sender, DateTime dateTime, LogLevel logLevel, string message)
        {
            Log(new LogMessage(sender, dateTime, logLevel, message));
        }

        public void Log(object sender, DateTime dateTime, LogLevel logLevel, string message, Exception innerException)
        {
            Log(new LogMessage(sender, dateTime, logLevel, message + "\r\nInner Exception: " + innerException.ToString()));
        }

        /// <summary>
        /// Log a message to a file (if FilePath is set) and execute the user's delegate (if set) with LogMessage info.
        /// </summary>
        /// <param name="logMessage"></param>
        public void Log(LogMessage logMessage)
        {
            if (logMessage == null)
                throw new ArgumentNullException("Logmessage can't be null.");
            //_Queue.Enqueue(logMessage)
            string formattedMessage = FormatMessage(logMessage, MessageFormat);
            // Log to file and to client-delegate
            if ((_FileLogLevels & logMessage.LogLevel) > 0)
                ToFile(formattedMessage);
            ToClient(logMessage, formattedMessage);
        }

        public static string FormatMessage(object sender, LogLevel logLevel, DateTime dateTime, string message, string format)
        {
            return FormatMessage(new LogMessage(sender, dateTime, logLevel, message), format);
        }

        public static string FormatMessage(LogMessage logMessage, string format)
        {
            // Format
            return format.Replace(DateFormatTag, logMessage.DateTime.ToString())
                         .Replace(LogLevelFormatTag, logMessage.LogLevel.ToString())
                         .Replace(SourceFormatTag, SenderToName(logMessage.Sender))
                         .Replace(MessageFormatTag, logMessage.Message);
        }

        public static string SenderToName(object sender)
        {
            // Get sender name
            string senderName = null;
            if (sender is string)
            {
                senderName = (string)sender;
            }
            else if (sender is Form)
            {
                Form form = (Form)sender;
                // invoke (getting these properties isn't threadsafe)
                if (form.InvokeRequired)
                {
                    senderName = form.Invoke(new Func<string>(() => { return form.Name + form.Handle.ToString(); })).ToString();
                }
                else
                {
                    senderName = form.Name + form.Handle.ToString();
                }
            }
            else
            {
                senderName = sender.GetType().Name;
            }
            return senderName;
        }

        private void ToFile(string formattedMessage)
        {
            // Get the filepath while it isn't altered
            string safeFilePath = FilePath;
            // write to file
            if (!string.IsNullOrEmpty(safeFilePath))
            {
                // create file (if it doesn't already exists)
                IO.FileUtils.CreateFile(safeFilePath);
                // write (append)
                try
                {
                    lock (_ThreadSafeFileWrite)
                    {
                        System.IO.StreamWriter writeStream = new System.IO.StreamWriter(safeFilePath, append: true);
                        writeStream.WriteLine(formattedMessage);
                        writeStream.Flush();
                        writeStream.Close();
                        writeStream.Dispose();
                    }
                }
                catch
                {
                }
            }
        }

        private void ToClient(LogMessage logMessage, string formattedLogMessage)
        {
            //' Dont wait for it
            //' If you raise asynchronously, multiple listeners would be processing your event at the same time. 
            //' At the very least, that requires a thread-safe EventArg class.
            //ThreadPool.QueueUserWorkItem(
            //    Sub()

            lock (_ThreadSafeClient)
            {
                // Thread safe property usage
                // Run client programmer's method asynchronously
                //LogMethod?.BeginInvoke(logMessage, formattedLogMessage, Nothing, Nothing)
                LogMethod?.Invoke(logMessage, formattedLogMessage);
                // Event
                OnNewLogMessage(this, new LoggerEventArgs(logMessage, formattedLogMessage));
            }

            //    End Sub)
        }
        #endregion

    }
}
