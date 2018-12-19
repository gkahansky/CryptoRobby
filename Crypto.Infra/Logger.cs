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
        public string reportPath { get; set; }
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

        public void InitializeStatsReport()
        {
            this.reportPath = pathString + "TransactionStats_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            using (StreamWriter sw = File.AppendText(reportPath))
            {
                var msg = "Pair,Pattern Name, Interval, Retention, Threshold, DefaultStopLoss,DynamicStopLoss,Total Profit,Number of Deals,Max Profit,Min Profit,First transaction,Last transaction";
                sw.WriteLine(msg);
            }
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
            await Log(msg, severity);
        }

        public async Task Debug(string msg)
        {
            await Log(msg, 0);
        }

        public async Task Info(string msg)
        {
            await Log(msg, 1);
        }

        public async Task Warning(string msg)
        {
            await Log(msg, 2);
        }

        public async Task Error(string msg)
        {
            await Log(msg, 3);
        }

        private async Task Log(string msg, int severity = 1)
        {
            CheckDateChange();
            using (StreamWriter sw = File.AppendText(path))
            {
                var now = DateTime.Now.ToString("yyy-MM-dd hh:mm:ss\t");

                var logPrefix = now + "\tINFO\t";
                switch (severity)
                {
                    case 0:
                        logPrefix = now + "\tDEBUG\t";
                        break;
                    case 1:
                        logPrefix = now + "\tINFO\t";
                        Console.WriteLine(now + msg);
                        break;
                    case 2:
                        logPrefix = now + "\tWARNING\t";
                        Console.WriteLine(now + msg);
                        break;
                    case 3:
                        logPrefix = now + "\tERROR\t";
                        Console.WriteLine(now + msg);
                        break;
                    default:
                        Console.WriteLine(now + msg);
                        break;
                }
                if(severity >= Config.LogSeverity)
                    await sw.WriteLineAsync(logPrefix + msg);
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
                Error(e.ToString());
            }

        }


        public void Stats(string msg)
        {
            using (StreamWriter sw = File.AppendText(reportPath))
            {
                sw.WriteLine(msg);
            }
        }
    }
}
