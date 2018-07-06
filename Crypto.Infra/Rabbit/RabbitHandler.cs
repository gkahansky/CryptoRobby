using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using CryptoRobert.Infra;

namespace CryptoRobert.Infra.Rabbit
{
    public class RabbitHandler : IRabbitHandler
    {

        private readonly ILogger _logger;
        private IBasicProperties Properties { get; set; }
        private IModel Model { get; set; }
        private string Exchange { get; set; }

        public RabbitHandler(ILogger logger, string exchange)
        {
            _logger = logger;
            Exchange = exchange;
            _logger.Info("RabbitHandler Initiating");
        }


        public void Connect()
        {
            try
            {
                var connectionFactory = new ConnectionFactory() { HostName = Config.RabbitHost, UserName = Config.RabbitUser, Password = Config.RabbitPass };
                var connection = connectionFactory.CreateConnection();
                Model = connection.CreateModel();

                Properties = Model.CreateBasicProperties();
                Properties.Persistent = false;

                Model.ExchangeDeclare(this.Exchange, ExchangeType.Topic);

                _logger.Info("RabbitMQ Connection Established.");
            }
            catch (Exception e)
            {
                _logger.Info("Connection to RabbitMQ Failed.\n" + e.ToString());
                throw;
            }
        }

        public void PublishMessage(string msg)
        {
            byte[] payload = Encoding.Default.GetBytes(msg);
            this.Model.BasicPublish(Config.BnbExchange, "#", Properties, payload);
        }

        public void PublishKlineList(List<Kline> list)
        {
            try
            {
                foreach (var k in list)
                {
                    var kString = ConvertKlineToString(k);
                    PublishMessage(kString);
                }
            }
            catch (Exception e)
            {
                _logger.Info("Failed to publish klines to RabbitMQ.\n" + e.ToString());
            }

        }

        public void KlineListener(object sender, EventArgs e)
        {
            MetaDataContainer.klinePush += MetaDataContainer_klinePush;

        }

        private void MetaDataContainer_klinePush(object sender, EventArgs e)
        {
            _logger.LogAsync("new kline update arrived");
        }

        private string ConvertKlineToString(Kline kline)
        {
            var msg = "{";
            msg += " \"Symbol\" : \"" + kline.Symbol + "\",";
            msg += " \"Interval\" : \"" + kline.Interval + "\",";
            msg += " \"OpenTime\" : \"" + kline.OpenTime + "\",";
            msg += " \"CloseTime\" : \"" + kline.CloseTime + "\",";
            msg += " \"Open\" : \"" + kline.Open + "\",";
            msg += " \"Close\" : \"" + kline.Close + "\",";
            msg += " \"High\" : \"" + kline.High + "\",";
            msg += " \"Low\" : \"" + kline.Low + "\",";
            msg += " \"Volume\" : \"" + kline.Volume + "\"";
            msg += " }";
            return msg;
        }

    }
}
