using System;
using System.Collections.Generic;
using CryptoRobert.Infra;

namespace CryptoRobert.RuleEngine.Data
{
    internal class Unsubscriber : IDisposable
    {
        private List<IObserver<Kline>> _observers;
        private IObserver<Kline> _observer;

        public Unsubscriber(List<IObserver<Kline>> observers, IObserver<Kline> observer)
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