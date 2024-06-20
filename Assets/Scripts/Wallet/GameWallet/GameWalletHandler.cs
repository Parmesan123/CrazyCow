using ToolBox.Serialization;
using UI;

namespace Handlers
{
	public class GameWalletHandler : BaseWalletHandler
	{
		public void Register(Coin coin)
		{
			coin.OnDestroyEvent += CoinListener;
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

		public override void Save()
		{
			if (!DataSerializer.TryLoad(COINS_SAVE_KEY, out WalletSaveData walletData))
				walletData = new WalletSaveData(0);
			walletData.MoneyCount += _coins;
			
			DataSerializer.Save(COINS_SAVE_KEY, walletData);
		}
	}
}