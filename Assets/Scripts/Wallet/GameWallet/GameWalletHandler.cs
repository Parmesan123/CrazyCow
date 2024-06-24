using Saving;

namespace Wallet
{
	public class GameWalletHandler : BaseWalletHandler
	{
		protected override void CoinListener(Coin coin)
		{
			coin.OnDestroyEvent -= CoinListener;

			_coins += 1;
			UpdateCoins();
		}

		public void SerializeInGlobalWallet(GameData data) => data.MoneyCount += _coins;
	}
}