using System;

namespace Sverto.General.Logging
{
    public class LogMessage
    {
        public LogMessage(object sender, DateTime dateTime, LogLevel logLevel, string message)
        {
            if (sender == null)
                throw new ArgumentNullException("Sender can't be null.");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message can't be empty.");

            Sender = sender;
            DateTime = dateTime;
            LogLevel = logLevel;
            Message = message;
        }

        public object Sender { get; }
        public DateTime DateTime { get; }
        public LogLevel LogLevel { get; }
        public string Message { get; }

        public override string ToString()
        {
            return LogLevel.ToString() + " - " + Sender.ToString() + " - " + Message;
        }

    }
}
