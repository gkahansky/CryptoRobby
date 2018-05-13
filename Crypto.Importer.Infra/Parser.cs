using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Crypto.Infra;



namespace Crypto.Importer.Infra
{
    public static class Parser
    {
        public static List<JObject> ParseCmcCoins(List<string> coinSetList)
        {
            var list = new List<JObject>();
            foreach (var coinSet in coinSetList)
            {
                var newString = coinSet.Substring(coinSet.IndexOf('{'), coinSet.LastIndexOf('}') - coinSet.IndexOf('{') + 1);
                var jsons = newString.Split('{');
                Logger.Log(newString);

                foreach (var json in jsons)
                {
                    if (json.Length > 2)
                    {
                        var newJson = "{" + json.Substring(0, json.LastIndexOf('}') + 1);
                        Logger.Log(newJson);
                        var newObj = JObject.Parse(newJson);
                        list.Add(newObj);
                    }
                }
            }
            return list;
        }

    }
}
