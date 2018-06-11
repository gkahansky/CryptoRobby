using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Crypto.Infra
{
    public class Logger : ILogger
    {
        const string pathString = @"C:\Crypto\Log\";
        public string path { get; set; }

        public Logger(string source)
        {
            this.path = pathString + source + "_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".txt";
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


                client.Send(message);
                Log(body);
            }
            catch(Exception e)
            {
                Log(e.ToString());
            }
        }

    }
}
