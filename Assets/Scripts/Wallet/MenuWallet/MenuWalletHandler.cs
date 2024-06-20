using ToolBox.Serialization;

public class MenuWalletHandler : BaseWalletHandler
{
    private WalletSaveData _data;

    private void Awake()
    {
        if (!DataSerializer.TryLoad(COINS_SAVE_KEY, out _data))
            _data = new WalletSaveData(0);
    }

    public override void UpdateCoins(int count)
    {
        base.UpdateCoins(count);
        
        _data.MoneyCount = _coins;
    }

    public override void Save()
    {
        DataSerializer.Save(COINS_SAVE_KEY, _data);
    }
}