using UI;

namespace Handlers
{
	public class GameWalletHandler : BaseWalletHandler, IBind<WalletSaveData>
	{
		public string Id => nameof(GameWalletHandler);
		private WalletSaveData _walletData;
			
		public void Register(Coin coin)
		{
			coin.OnDestroyEvent += CoinListener;
		}
		
		public void Bind(WalletSaveData data)
		{
			data.Id = Id;
		}

		public void SaveData()
		{
			_walletData.MoneyCount += _coins;
		}

		private void UnRegister(Coin coin)
		{
			coin.OnDestroyEvent -= CoinListener;
		}

		private void CoinListener(Coin coin)
		{
			UnRegister(coin);
			
			UpdateCoins(1);
		}
	}
}