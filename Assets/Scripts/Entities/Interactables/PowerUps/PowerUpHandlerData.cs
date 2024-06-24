using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "PowerUpHandler", menuName = "ScriptableObjects/Data/PowerUpHandler")]
    public class PowerUpHandlerData : ScriptableObject
    {
        [field: SerializeField, Range(0f, 1f)] public float PowerUpSpawnChance;
    }
}