using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;

namespace Handlers
{
	public class CoinHandler
	{
		private TextMeshProUGUI _text;
		private int _wallet;

		public CoinHandler(TextMeshProUGUI text)
		{
			_text = text;
			_text.text = _wallet.ToString();
		}
		
		public void Register(Coin coin)
		{
			coin.OnDestroyEvent += CoinListener;
		}

		public void UnRegister(Coin coin)
		{
			coin.OnDestroyEvent -= CoinListener;
		}

		private void CoinListener(Coin coin)
		{
			UnRegister(coin);
			++_wallet;
			UpdateWalletUI();
		}

		private void UpdateWalletUI() // TODO create other self
		{
			_text.text = _wallet.ToString();
			_text.transform.localScale = Vector3.one;
			_text.transform.DOKill();
			_text.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f, 1);
		}
	}
}