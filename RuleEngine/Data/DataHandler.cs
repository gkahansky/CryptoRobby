﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Linq;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Patterns;
using Crypto.RuleEngine.Entities;
using Crypto.RuleEngine.Data;
using CryptoRobert.RuleEngine.Interfaces;

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

        public List<RuleDefinition> LoadRulesFromDb()
        {
            try
            {
                var list = new List<RuleDefinition>();

                using (var context = new RuleContext())
                {
                    list = context.RuleDefinitions.ToList();
                }

                return list;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to Load Rules from Database.\n" + e);
                throw;
            }
        }

        public List<RuleSetDefinition> LoadRuleSetsFromDb()
        {
            try
            {
                var list = new List<RuleSetDefinition>();

                using (var context = new RuleContext())
                {
                    list = context.RuleSetDefinitions.ToList();
                }

                return list;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to Load Rule Sets from Database.\n" + e);
                throw;
            }
        }


        public RuleDefinition GetRuleById(int id)
        {

            try
            {
                using (var context = new RuleContext())
                {
                    var rule = (RuleDefinition)context.RuleDefinitions.Where(r => r.Id == id);
                    return rule;
                }
            }
            catch (Exception e)
            {
                _logger.Error("Failed to Retrieve Rule with Id: " + id + e);
                return null;
            }
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
                _logger.Info("Failed to save Patterns.\n" + e.ToString());
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
                _logger.Info("Failed to retreive list of klines from csv file.\n" + e.ToString());
                return null;
            }
        }


    }
}
