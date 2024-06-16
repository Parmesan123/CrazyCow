using System;
using UI;

namespace Handlers
{
	public class WalletHandler
	{
		public event Action<int> OnUIUpdateEvent; 
			
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
			++_coins;
			OnUIUpdateEvent?.Invoke(_coins);
		}
	}
}