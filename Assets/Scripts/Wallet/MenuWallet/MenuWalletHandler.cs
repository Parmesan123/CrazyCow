using System;
using Zenject;

public class MenuWalletHandler 
{
    public event Action OnCashSpendFailedEvent;
    public event Action<int> OnCashUpdatedEvent;

    private readonly GameData _data;
    private int _coins;

    [Inject]
    private MenuWalletHandler(SaveHandler saveHandler)
    {
        _data = saveHandler.SaveData;
        
        _coins = _data.MoneyCount;
    }
    
    public bool TrySpend(int count)
    {
        if (_coins < count)
        {
            OnCashSpendFailedEvent.Invoke();
            return false;
        }

        _coins -= count;
        OnCashUpdatedEvent.Invoke(_coins);
        UpdateGlobalWallet();
        return true;
    }

    public void UpdateCoins()
    {
        OnCashUpdatedEvent.Invoke(_coins);
    }

    private void UpdateGlobalWallet()
    {
        _data.MoneyCount = _coins;
    }
}