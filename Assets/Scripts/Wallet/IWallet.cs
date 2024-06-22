using System;
using UI;

public interface IWallet
{
    public event Action<int> OnCashUpdatedEvent;

    public void Register(Coin coin);
}