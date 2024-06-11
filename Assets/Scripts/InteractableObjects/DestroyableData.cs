using NaughtyAttributes;
using UnityEngine;

namespace InteractableObject
{
	[CreateAssetMenu(fileName = "DestroyableData", menuName = "ScriptableObjects/Data/DestroyableData")]
	public class DestroyableData : ScriptableObject
	{
		[field: SerializeField] public Material CrossSectionMaterial { get; private set; }
		[field: SerializeField] public float TimeToDestroy { get; private set; }
		[SerializeField, MinMaxSlider(0,5)] private Vector2 _lifetime;
		public float Lifetime => Random.Range(_lifetime.x, _lifetime.y);
		
		[SerializeField, MinMaxSlider(1, 15)] private Vector2Int _amountGivenCoin;
		public int AmountCoin => Random.Range(_amountGivenCoin.x, _amountGivenCoin.y);
	}
}