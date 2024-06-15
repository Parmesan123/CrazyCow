using UnityEngine;

namespace Entities
{
	[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/Data/CharacterData")]
	public class CharacterData : ScriptableObject
	{
		[field: SerializeField] public float CharacterSpeed { get; private set; }
		[field: SerializeField] public float DestroyRange { get; private set; }
	}
}