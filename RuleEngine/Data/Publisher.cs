using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Data
{
    public class Publisher : IObservable<Kline>
    {
        List<IObserver<Kline>> observers;

        public Publisher()
        {
            observers = new List<IObserver<Kline>>();
        }

        public IDisposable Subscribe(IObserver<Kline> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }
    }
}
