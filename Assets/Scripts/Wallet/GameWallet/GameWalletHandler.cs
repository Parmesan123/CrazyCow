using System;
using UI;

namespace Handlers
{
	public class GameWalletHandler
	{
		public event Action<int> OnCoinsUpdateEvent;
	
		private int _coins;
		
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

			_coins += 1;
			OnCoinsUpdateEvent.Invoke(_coins);
		}

		public void SerializeInGlobalWallet(GameData data) => data.MoneyCount += _coins;
	}
}