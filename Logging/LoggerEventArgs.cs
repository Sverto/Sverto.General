using System;

namespace Sverto.General.Logging
{
    public class LoggerEventArgs : EventArgs
    {

        public LoggerEventArgs(LogMessage logMessage, string formattedLogMessage)
        {
            _LogMessage = logMessage;
            _FormattedLogMessage = formattedLogMessage;
        }

        private readonly LogMessage _LogMessage;
        private readonly string _FormattedLogMessage;

        private readonly object _ThreadSafe = new object();
        public LogMessage LogMessage
        {
            get
            {
                lock (_ThreadSafe)
                {
                    return _LogMessage;
                }
            }
        }

        public string FormattedLogMessage
        {
            get
            {
                lock (_ThreadSafe)
                {
                    return _FormattedLogMessage;
                }
            }
        }

    }
}
