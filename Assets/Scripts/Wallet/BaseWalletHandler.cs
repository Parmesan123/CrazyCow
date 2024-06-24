using System;
using UI;

namespace Wallet
{
    public abstract class BaseWalletHandler : IWallet
    {
        public event Action<int> OnCashUpdatedEvent;
    
        protected int _coins;
    
        public void UpdateCoins()
        {
            OnCashUpdatedEvent.Invoke(_coins);
        }
    
        public void Register(Coin coin)
        {
            coin.OnDestroyEvent += CoinListener;
        }

        protected abstract void CoinListener(Coin coin);
    }
}