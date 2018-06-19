using Crypto.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine.Data
{
    public class KlineReporter : IObserver<Kline>
    {
        private IDisposable unsubscriber;
        private bool first = true;
        private Kline last;
        private ILogger _logger;

        public KlineReporter(ILogger logger)
        {
            _logger = logger;
        }

        public virtual void Subscribe(IObservable<Kline> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }

        public virtual void OnNext(Kline value)
        {
            if (first)
            {
                last = value;
                first = false;
            }
            else
            {
               
            }
        }

        public void OnCompleted()
        {
            _logger.Log("KlineReporter Completed");
        }

        public void OnError(Exception e)
        {
            _logger.Log("An error occured while trying to receive kline updates from publisher.\n" + e.ToString());
        }
    }
}
