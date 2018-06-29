using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryptoRobert.Infra
{
    public class Logger : ILogger
    {
        const string pathString = @"C:\Crypto\Log\";
        public string path { get; set; }
        private string Source { get; set; }

        public Logger(string source)
        {
            this.Source = source;
            GenerateLogPath();
        }

        private void GenerateLogPath()
        {
            this.path = pathString + Source + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
        }

        private void CheckDateChange()
        {
            var logDate = path.Substring(path.IndexOf('_') + 7, 2);
            var today = DateTime.Now.ToString("dd");
            if (logDate != today)
            {
                GenerateLogPath();
            }
        }

        public async Task LogAsync(string msg, int severity = 1)
        {

            using (StreamWriter sw = File.AppendText(path))
            {
                var now = DateTime.Now.ToString("yyy-MM-dd hh:mm:ss\t");
                sw.WriteLine(now + msg);
                switch (severity)
                {
                    case 0:
                        break;
                    default:
                        Console.WriteLine(now + msg);
                        break;
                }

            }
        }

        public void Log(string msg, int severity = 1)
        {
            CheckDateChange();
            using (StreamWriter sw = File.AppendText(path))
            {
                var now = DateTime.Now.ToString("yyy-MM-dd hh:mm:ss\t");
                sw.WriteLine(now + msg);
                switch (severity)
                {
                    case 0:
                        break;
                    default:
                        Console.WriteLine(now + msg);
                        break;
                }

            }
        }

        public void Email(string subject, string body)
        {
            try
            {
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Host = "smtp.gmail.com";
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("Shlomansky", "gl23TN45!");
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage("Shlomansky@gmail.com", "Shlomansky@gmail.com");
                message.Subject = subject;
                message.Body = body;

                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;

                if (!Config.TestMode)
                    client.Send(message);
                Log(body);
            }
            catch (Exception e)
            {
                Log(e.ToString());
            }

        }

    }
}
