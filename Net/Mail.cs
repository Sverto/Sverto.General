using System;
using System.Net.Mail;
using System.Text;

namespace Sverto.General.Net
{
    public class Mail
    {

        public static bool SendMail(string smtpServer, int smtpPort, string username, string password, MailMessage mail)
        {
            SmtpClient smtp = new SmtpClient(smtpServer);
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(username, password);
            smtp.Port = smtpPort;
            try
            {
                smtp.Send(mail);
            }
            catch
            {
                return false;
            }
            return true;
        }

        // Encode text to a safe html string
        // TODO: There better ways to do this now
        private static string HtmlEncodeAll(string value)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char chr in value)
            {
                builder.Append("&#");
                builder.Append(Convert.ToInt32(chr));
                builder.Append(";");
            }
            return builder.ToString();
        }

    }
}
