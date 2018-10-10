﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Linq;
using CryptoRobert.Infra;
using CryptoRobert.RuleEngine.Entities;
using CryptoRobert.RuleEngine.Data;
using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.RuleEngine.Entities.MetaData;
using Microsoft.EntityFrameworkCore;

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

        public List<RuleDefinition> LoadRulesFromDb(int id = 0)
        {
            try
            {
                var list = new List<RuleDefinition>();
                if (id == 0)
                {
                    using (var context = new RuleContext())
                    {
                        list = context.RuleDefinitions.ToList();
                    }
                }
                else
                {
                    using (var context = new RuleContext())
                    {
                        list = context.RuleDefinitions.Where(r => r.Id==id).ToList();
                    }
                }


                return list;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to Load Rules from Database.\n" + e);
                throw;
            }
        }

        public List<RuleSetDefinition> LoadRuleSetToRulesFromDb()
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

        public List<RuleSet> LoadRuleSetsFromDb(int id = 0)
        {
            var sets = new List<RuleSet>();

            if (id == 0)
            {
                using (var context = new RuleContext())
                {
                    sets = context.RuleSets.ToList();
                }
            }
            else
            {
                using (var context = new RuleContext())
                {
                    sets = context.RuleSets.Where(s => s.Id == id).ToList();
                }
            }
            
            return sets;
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

        public List<Pair> LoadCoinPairsFromDb()
        {
            List<Pair> pairs = new List<Pair>();

            using(var context = new RuleContext())
            {
                pairs = context.Pairs.FromSql("SELECT Id, Symbol FROM CoinPairs").ToList();
            }

            return pairs;
        }

        public bool SaveRuleSet(RuleSet set)
        {
            try
            {
                using (var context = new RuleContext())
                {
                    context.Add(set);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to save Rule Set.\n" + e);
                return false;
            }
            
        }
    }
}
