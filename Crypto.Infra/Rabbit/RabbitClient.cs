﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Threading;

namespace Crypto.Infra.Rabbit
{
    public class RabbitClient
    {
        private readonly ILogger _logger;
        private IBasicProperties Properties { get; set; }
        private IModel Model { get; set; }
        private string QueueName { get; set; }
        private string[] Exchanges { get; set; }
        private IConnection Connection { get; set; }
        private EventingBasicConsumer Consumer { get; set; }

        public RabbitClient(ILogger logger, string queueName, string[] exchanges)
        {
            _logger = logger;
            QueueName = queueName;
            Exchanges = exchanges;
            _logger.Log("Rabbit Client Initialized.");
        }

        public IModel Connect()
        {
            try
            {
                var connectionFactory = new ConnectionFactory() { HostName = Config.RabbitHost, UserName = Config.RabbitUser, Password = Config.RabbitPass };
                Connection = connectionFactory.CreateConnection();
                Model = Connection.CreateModel();

                Properties = Model.CreateBasicProperties();
                Properties.Persistent = false;

                InitializeQueues(Exchanges);

                _logger.Log("RabbitMQ Connection Established.");
                return Model;
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

        public void InitializeConsumer(string queue, IModel model)
        {

            Consumer = new EventingBasicConsumer(model);
            Consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                // ... process the message
                var jsonString = Encoding.Default.GetString(ea.Body);
                var kline = JsonConvert.DeserializeObject<Kline>(jsonString);
                _logger.Log(kline.Symbol + "_" + kline.Interval + "Received from Exchange");
                //
                model.BasicAck(ea.DeliveryTag, false);
            };
            String consumerTag = model.BasicConsume(queue, false, Consumer);
        }

        public void Dispose()
        {
            Connection.Close();
            Model.Close();
        }
    }
}
