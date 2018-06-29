using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;
using RabbitMQ.Client;

namespace CryptoRobert.Infra.Rabbit
{
    public interface IRabbitHandler
    {
        void Connect();

        void PublishMessage(string msg);
        void PublishKlineList(List<Kline> klineList);
    }
}
