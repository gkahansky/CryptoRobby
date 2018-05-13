using System;
using System.Collections.Generic;
using System.Text;
using Crypto.Infra;


namespace Crypto.Importer.Infra
{
    public static class ImporterDbHandler
    {
        private static void SaveCoin(string symbol, string name)
        {
            try
            {
                var coin = new Coin(symbol, name);
                using (var context = new InfraContext())
                {
                    context.Coins.Add(coin);
                    context.SaveChanges();
                }
                Logger.Log(String.Format("New Coin Saved: Id={0}, Symbol = {1}, Name={2}", coin.Id, coin.Symbol, coin.Name));
            }
            catch (Exception e)
            {
                Logger.Log("Failed to save coin to database.\n" + e.ToString());
            }
        }
    }
}
