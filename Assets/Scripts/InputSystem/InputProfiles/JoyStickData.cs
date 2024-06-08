using UnityEngine;

namespace InputSystem
{
	[CreateAssetMenu(fileName = "JoyStickData", menuName = "ScriptableObjects/Data/JoyStickData")]
	public class JoyStickData : ScriptableObject
	{
		[field: SerializeField] public float JoyStickRadius { get; private set; }
		[field: SerializeField] public float DeadZone { get; private set; }
	}
}