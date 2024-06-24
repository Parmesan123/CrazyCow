using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wallet
{
	[CreateAssetMenu(fileName = "CoinData", menuName = "ScriptableObjects/Data/CoinData")]
	public class CoinData : ScriptableObject
	{
		[SerializeField, MinMaxSlider(0.1f, 2f)] private Vector2 _scaleDuration; 
		[SerializeField, MinMaxSlider(0.1f, 5)] private Vector2 _moveDuration;

		public float ScaleDuration => Random.Range(_scaleDuration.x, _scaleDuration.y);
		public float MoveDuration => Random.Range(_moveDuration.x, _moveDuration.y);
	}
}