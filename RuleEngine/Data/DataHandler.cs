using System;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Data;
using System.Collections.Generic;
using System.IO;
using CryptoRobert.Infra.Patterns;
using System.Data.SqlClient;

namespace CryptoRobert.RuleEngine
{
    public class DataHandler : IDataHandler
    {
        private ILogger _logger;
        private string ConnectionString;
        public DataHandler(ILogger logger)
        {
            _logger = logger;
            ConnectionString = Config.SqlConnectionString;
        }

        public void LoadCoinDataFromDb()
        {
            new NotImplementedException();
        }

        public void SavePatterns(Dictionary<string, IPattern> repo)
        {
            SqlConnection con = new SqlConnection(this.ConnectionString);
            
            try
            {
                con.Open();
                foreach (var item in repo)
                {
                    var p = new PatternView(item.Value);
                    var commandString = string.Format(@"EXECUTE [dbo].[SavePatterns] '{0}', '{1}', '{2}', {3}, {4}", p.Name, p.Symbol, p.Interval, p.DefaultStopLossThreshold, p.DynamicStopLossThreshold);
                    SqlCommand command = new SqlCommand(commandString, con);
                    command.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception e)
            {
                _logger.Log("Failed to save Patterns.\n" + e.ToString());
                throw;
            }
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
