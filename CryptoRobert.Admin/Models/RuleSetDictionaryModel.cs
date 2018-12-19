using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CryptoRobert.Admin.Models
{
    public class RuleSetDictionaryModel
    {
        public Dictionary<int, RuleSetModel> Sets { get; set; }

        public RuleSetDictionaryModel()
        {
            Sets = new Dictionary<int, RuleSetModel>();
        }
    }
}