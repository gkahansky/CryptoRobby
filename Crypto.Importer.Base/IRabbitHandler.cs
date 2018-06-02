using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Crypto.Importer.Base
{
    public interface IRabbitHandler
    {
        void Connect();

        void PublishMessage(string msg);
    }
}
