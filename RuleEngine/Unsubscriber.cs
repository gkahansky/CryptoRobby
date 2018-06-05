using System;
using System.Collections.Generic;
using Crypto.Infra;

namespace Crypto.RuleEngine
{
    internal class Unsubscriber : IDisposable
    {
        private List<IObserver<CoinPair>> _observers;
        private IObserver<CoinPair> _observer;

        public Unsubscriber(List<IObserver<CoinPair>> observers, IObserver<CoinPair> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}