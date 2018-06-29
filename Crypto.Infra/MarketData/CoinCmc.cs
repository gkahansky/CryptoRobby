using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Infra
{
    public class CoinCmc
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Rank { get; set; }
        public decimal PriceUsd { get; set; }
        public decimal PriceBtc { get; set; }
        public decimal VolumeUsd { get; set; }
        public decimal MarketCapUsd { get; set; }
        public decimal AvailableSupply { get; set; }
        public decimal TotalSupply { get; set; }
        public decimal MaxSupply { get; set; }
        public decimal ChangePct1Hr { get; set; }
        public decimal ChangePct24Hr { get; set; }
        public decimal ChangePct7d { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
