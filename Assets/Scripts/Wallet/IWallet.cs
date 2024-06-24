using System;
using UI;

namespace Wallet
{
    public interface IWallet
    {
        public event Action<int> OnCashUpdatedEvent;

        public void Register(Coin coin);
    }
}