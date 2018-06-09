using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Crypto.Infra;

namespace Crypto.Importer.Base
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
            _logger.Log("RabbitHandler Initiating");
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

                _logger.Log("RabbitMQ Connection Established.");
            }
            catch (Exception e)
            {
                _logger.Log("Connection to RabbitMQ Failed.\n" + e.ToString());
                throw;
            }
        }

        public void PublishMessage(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
