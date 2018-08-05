using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CryptoRobert.Infra;
using CryptoRobert.RuleEngine;

namespace RuleTester
{
    public class FileAnalyzer
    {
        private ILogger logger;

        public FileAnalyzer(ILogger _logger)
        {
            logger = _logger;
        }


        public List<Kline> GenerateKlinesFromCsv(ILogger logger, string path)
        {
            var data = new DataHandler(logger);
            var coinData = data.LoadCoinDataFromCsv(path);

            Parser parser = new Parser(logger);
            var klineList = parser.ParseKlinesFromCsvToList(coinData);

            return klineList;
        }

        public Tuple<Dictionary<string, string>, Dictionary<string, string>> AnalyzeFileData(string path)
        {
            var symbols = new Dictionary<string,string>();
            var intervals = new Dictionary<string, string>();
            var contentList = new Tuple<Dictionary<string, string>, Dictionary<string, string>>(symbols, intervals);
            var lines = new List<string>();

            lines = ReadFileContent(path);
            try
            {
                foreach (var line in lines)
                {
                    if (!line.StartsWith("Symbol"))
                    {
                        var symbol = line.Substring(0, (line.IndexOf(",")));
                        var intervalstring = line.Substring(line.IndexOf(",")+1);
                        var interval = intervalstring.Substring(0, intervalstring.IndexOf(","));

                        if (!symbols.ContainsKey(symbol))
                            symbols.Add(symbol, symbol);
                        if (!intervals.ContainsKey(interval))
                            intervals.Add(interval, interval);
                    }
                }
                return contentList;
            }
            catch (Exception e)
            {
                logger.Error("Failed to Analyze File Data.\n" + e.ToString());
                var s = new Dictionary<string, string>();
                var i = new Dictionary<string, string>();
                var l = new Tuple<Dictionary<string, string>, Dictionary<string, string>>(s,i);
                return l;
            }
        }

        private List<string> ReadFileContent(string path)
        {
            var lines = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string currentLine;
                    // currentLine will be null when the StreamReader reaches the end of file
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        lines.Add(currentLine);
                    }
                }

                return lines;
            }
            catch (Exception e)
            {
                logger.Error("Failed to analyze file content for '" + path + "'\n" + e.ToString());
                return lines;
            }
            
        }
    }
}
