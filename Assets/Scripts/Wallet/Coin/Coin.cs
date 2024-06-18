using System;
using UnityEngine;

namespace UI
{
	public class Coin : MonoBehaviour
	{
		public event Action<Coin> OnDestroyEvent;

		[field: SerializeField] public CoinData CoinData { get; private set; }

		private void OnDestroy()
		{
			OnDestroyEvent.Invoke(this);
		}
	}
}