using NaughtyAttributes;
using UnityEngine;

namespace InteractableObject
{
	[CreateAssetMenu(fileName = "DestroyableData", menuName = "ScriptableObjects/Data/DestroyableData")]
	public class DestroyableData : ScriptableObject
	{
		[field: SerializeField] public float TimeToDestroy { get; private set; }
		[SerializeField, MinMaxSlider(1, 15)] private Vector2Int _amountGivenCoin;

		public int AmountCoin => Random.Range(_amountGivenCoin.x, _amountGivenCoin.y);
	}
}