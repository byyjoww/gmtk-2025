using SLS.Core;
using System;

namespace GMTK2025.Utils
{
    public abstract class Tickable : ITickable, IDisposable
    {
        private ITicker ticker = default;

        public Tickable(ITicker ticker)
        {
            this.ticker = ticker;
            ticker.RegisterOnTick(this);
        }

        void ITickable.OnTick()
        {
            OnTick();
        }

        protected abstract void OnTick();

        protected virtual void OnDispose()
        {

        }

        public void Dispose()
        {
            ticker.DeregisterOnTick(this);
            OnDispose();
        }
    }
}
