using System;
using UI;
using Zenject;

public class MenuWalletHandler : BaseWalletHandler
{
    public event Action OnCashSpendFailedEvent;

    private readonly SaveHandler _saveHandler;

    [Inject]
    private MenuWalletHandler(SaveHandler saveHandler)
    {
        _saveHandler = saveHandler;
        
        _coins = saveHandler.SaveData.MoneyCount;
    }
    
    public bool TryRemoveCoins(int count)
    {
        if (_coins < count)
        {
            OnCashSpendFailedEvent.Invoke();
            return false;
        }

        _coins -= count;
        UpdateCoins();
        UpdateGlobalWallet();
        return true;
    }
    
    protected override void CoinListener(Coin coin)
    {
        coin.OnDestroyEvent -= CoinListener;

        _coins += 1;
        UpdateCoins();
        UpdateGlobalWallet();
        _saveHandler.Save();
    }

    private void UpdateGlobalWallet()
    {
        _saveHandler.SaveData.MoneyCount = _coins;
    }
}