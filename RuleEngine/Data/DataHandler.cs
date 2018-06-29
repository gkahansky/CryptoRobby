using System;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Data;
using System.Collections.Generic;
using System.IO;

namespace CryptoRobert.RuleEngine
{
    public class DataHandler : IDataHandler
    {
        private ILogger _logger;

        public DataHandler(ILogger logger)
        {
            _logger = logger;
        }

        public void LoadCoinDataFromDb()
        {
            new NotImplementedException();
        }

        public List<string> LoadCoinDataFromCsv(string path)
        {
            try
            {
                List<string> lines = new List<string>();
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
                _logger.Log("Failed to retreive list of klines from csv file.\n" + e.ToString());
                return null;
            }
        }

    }
}
