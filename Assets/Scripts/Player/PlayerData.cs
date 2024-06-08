using UnityEngine;

namespace Player
{
	[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Data/PlayerData")]
	public class PlayerData : ScriptableObject
	{
		[field: SerializeField] public float PlayerSpeed { get; private set; }
		[field: SerializeField] public float DestroyRange { get; private set; }
	}
}