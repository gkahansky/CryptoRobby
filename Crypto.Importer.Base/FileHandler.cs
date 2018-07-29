using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CryptoRobert.Infra;

namespace CryptoRobert.Importer.Base
{
    public class FileHandler : IFileHandler
    {
        private string path;

        public FileHandler(ILogger logger)
        {
            path = @"C:\Crypto\BackTesting\BinanceTickData_" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".txt";
            SetHeader();
            logger.Info("Recording Tick Data to: " + path);
        }


        private void SetHeader()
        {
            var header = "Symbol,Interval,OpenTime,CloseTime,Open,Close,High,Low,Volume";
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(header);
            }
        }

        public void SaveKlineToFile(List<Kline> klines)
        {

            foreach(var kline in klines)
            {
                List<string> list = new List<string>();

                list.Add(kline.Symbol.ToString());
                list.Add(kline.Interval.ToString());
                list.Add(kline.OpenTime.ToString());
                list.Add(kline.CloseTime.ToString());
                list.Add(kline.Open.ToString());
                list.Add(kline.Close.ToString());
                list.Add(kline.High.ToString());
                list.Add(kline.Low.ToString());
                list.Add(kline.Volume.ToString());


                var content = PrintKlineToFile(list);

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(content);
                }
            }

        }

        private string PrintKlineToFile(List<string> list)
        {
            var content = "";
            foreach (var field in list)
            {
                content += field + ",";
            }

           var newContent = content.Remove(content.Length - 2);

            return newContent;
        }
    }

}
