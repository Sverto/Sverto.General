using System.Drawing;
using System.Text;

namespace Sverto.General.Logging
{
    public static class LoggingExtensions
    {

        // LogMessage
        public static string ToHtml(this LogMessage m, Color dateTimeColor, Color logLevelColor, Color messageColor)
        {
            StringBuilder htmlSb = new StringBuilder();
            // The compiler would actually use StringBuilder for concats automatically (except for loops)
            htmlSb.Append("<span style='color: ");
            htmlSb.Append(dateTimeColor.ToCss());
            htmlSb.Append(";'>");
            htmlSb.Append(Logger.DateFormatTag);
            htmlSb.Append("</span> <span style='color: ");
            htmlSb.Append(logLevelColor.ToCss());
            htmlSb.Append(";'>&lt;");
            htmlSb.Append(Logger.LogLevelFormatTag);
            htmlSb.Append("/");
            htmlSb.Append(Logger.SourceFormatTag);
            htmlSb.Append("&gt;</span> <span style='color: ");
            htmlSb.Append(messageColor.ToCss());
            htmlSb.Append(";'>");
            htmlSb.Append(Logger.MessageFormatTag);
            htmlSb.Append("</span>");
            return Logger.FormatMessage(m, htmlSb.ToString());
        }

        private static string ToCss(this Color c)
        {
            return string.Format("rgb({0},{1},{2})", c.R, c.G, c.B);
        }

        // LogLevel
        public static Color ToColor(this LogLevel l, Color info, Color warning, Color error, Color debug, Color @default)
        {
            if (l == LogLevel.Info)
                return info;
            if (l == LogLevel.Warning)
                return warning;
            if (l == LogLevel.Error)
                return error;
            if (l == LogLevel.Debug)
                return debug;
            return @default;
        }

    }
}
