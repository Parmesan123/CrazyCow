using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "PowerUpData", menuName = "ScriptableObjects/Data/PowerUpData")]
    public class PowerUpData : ScriptableObject
    {
        [field: SerializeField] public float Time { get; private set; }
        [field: SerializeField] public Material Material { get; private set; }
    }
}