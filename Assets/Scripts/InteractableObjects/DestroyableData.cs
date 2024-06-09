using UnityEngine;

namespace InteractableObject
{
	[CreateAssetMenu(fileName = "DestroyableData", menuName = "ScriptableObjects/Data/DestroyableData")]
	public class DestroyableData : ScriptableObject
	{
		[field: SerializeField] public float TimeToDestroy { get; private set; }
		[field: SerializeField] public int AmountGivenCoin { get; private set; }
	}
}