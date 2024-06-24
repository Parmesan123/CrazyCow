using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Wallet
{
	public class CoinCluster : MonoBehaviour
	{
		private List<Coin> _coins;
		private Vector3 _endPosition;
		
		public void Init(List<Coin> coins, Vector3 position)
		{
			_coins = coins;
			_endPosition = position;
			StartTween();
		}

		private void StartTween()
		{
			foreach (Coin coin in _coins)
			{
				coin.gameObject.SetActive(true);
				coin.transform.localScale = Vector3.zero;
				coin.transform.DOScale(Vector3.one, coin.CoinData.ScaleDuration)
					.OnComplete(() => coin.transform.DOMove(_endPosition, coin.CoinData.MoveDuration)
										  .OnComplete(() => Destroy(coin.gameObject)));
			}
		}
	}
}