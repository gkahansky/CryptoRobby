using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Crypto.Infra.Rabbit
{
    public class RabbitClient
    {
        private readonly ILogger _logger;
        private IBasicProperties Properties { get; set; }
        private IModel Model { get; set; }
        private IDictionary<string, List<string>> Subscriptions;
        private string QueueName { get; set; }
        private string[] Exchanges { get; set; }

        public RabbitClient(ILogger logger, string queueName, string[] exchanges)
        {
            _logger = logger;
            QueueName = queueName;
            Exchanges = exchanges;
            _logger.Log("Rabbit Client Initialized.");
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

                InitializeQueues(Exchanges);

                _logger.Log("RabbitMQ Connection Established.");
            }
            catch (Exception e)
            {
                _logger.Log("Connection to RabbitMQ Failed.\n" + e.ToString());
                throw;
            }
        }

        private void InitializeQueues(string[] exchanges)
        {
            try
            {
                if (exchanges.Count() > 0)
                {
                    Model.QueueDeclare(QueueName, true, true, false);
                    foreach (var e in exchanges)
                    {
                        Model.ExchangeDeclare(e, ExchangeType.Topic);
                        Model.QueueBind(QueueName, e, "#");
                        _logger.Log("Successfully Binded " + QueueName + " to exchange: " + e);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Log("Failed to Generate Queues.\n" + e.ToString());
            }
        }
    }
}
