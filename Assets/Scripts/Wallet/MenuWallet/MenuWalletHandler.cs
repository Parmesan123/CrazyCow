public class MenuWalletHandler : BaseWalletHandler
{
    private WalletSaveData _data;
    
    public override void UpdateCoins(int count)
    {
        base.UpdateCoins(count);
        
        _data.MoneyCount = _coins;
    }
}