public class MenuWalletHandler : BaseWalletHandler, IBind<WalletSaveData>
{
    public string Id => nameof(MenuWalletHandler);
    private WalletSaveData _data;
    
    public void Bind(WalletSaveData data)
    {
        _data = data;
        _data.Id = Id;
        _coins = data.MoneyCount;
    }

    public override void UpdateCoins(int count)
    {
        base.UpdateCoins(count);
        
        _data.MoneyCount = _coins;
    }
}