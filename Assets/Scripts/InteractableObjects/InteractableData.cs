using UnityEngine;

namespace InteractableObject
{
	[CreateAssetMenu(fileName = "InteractableData", menuName = "ScriptableObjects/Data/InteractableData")]
	public class InteractableData : ScriptableObject
	{
		[field: SerializeField] public float InteractTime { get; private set; }
	}
}