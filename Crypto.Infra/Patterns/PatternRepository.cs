﻿using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Infra.Patterns
{
    public class PatternRepository 
    {
        private ILogger _logger;
        public Dictionary<string, IPattern> Items { get; set; }

        public PatternRepository(ILogger logger)
        {
            _logger = logger;
            Items = new Dictionary<string, IPattern>();
            InitializePatterns(_logger);
        }

        private void InitializePatterns(ILogger _logger)
        {
            foreach(var p in Config.PatternsConfig)
            {
                var settings = p.Value;
                switch (p.Key)
                {
                    case "Spring":
                        {
                            foreach(var pair in Config.PairsToMonitor)
                            {
                                settings.Symbol = pair.Value;
                                settings.Interval = pair.Value;
                            }
                            IPattern pattern = new SpringPattern(_logger, settings);
                            Items.Add(pattern.Name, pattern);
                            _logger.Info("Spring Pattern Added to Repository");
                            break;
                        }
                    case "Streak":
                        {
                            IPattern pattern = new StreakPattern(_logger, settings);
                            Items.Add(pattern.Name, pattern);
                            _logger.Info("Streak Pattern Added to Repository");
                            break;
                        }
                    case "TrendShift":
                        {
                            IPattern pattern = new TrendShiftPattern(_logger, settings);
                            Items.Add(pattern.Name, pattern);
                            _logger.Info("TrendShift Pattern Added to Repository");
                            break;
                        }
                    case "TrendInclinePattern":
                        {
                            IPattern pattern = new TrendInclinePattern(_logger, settings);
                            Items.Add(pattern.Name, pattern);
                            _logger.Info("TrendInclinePattern Pattern Added to Repository");
                            break;
                        }

                        
                    default:
                        break;
                }
            }
        }

        public Dictionary<string, IPattern> GetPatterns()
        {
            return this.Items;
        }

        public void Add(IPattern p)
        {
            var hash = p.Symbol + "_" + p.Interval;
            if (!Items.ContainsKey(hash))
                Items.Add(hash, p);
        }
        
        public void Remove(IPattern p)
        {
            var hash = p.Symbol + "_" + p.Interval;
            if (Items.ContainsKey(hash))
                Items.Remove(hash);
        }

        public IPattern Find(string hash)
        {
            if (Items.ContainsKey(hash))
                return Items[hash];
            else
                return null;
        }
    }
}
