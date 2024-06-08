using UnityEngine;

namespace InteractableObject
{
	[CreateAssetMenu(fileName = "BoxData", menuName = "ScriptableObjects/Data/BoxData")]
	public class BoxData : ScriptableObject
	{
		[field: SerializeField] public float TimeToDestroy { get; private set; }
		[field: SerializeField] public int AmountMoneyGiven { get; private set; }
	}
}