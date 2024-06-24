using UnityEngine;

namespace Store
{
    [CreateAssetMenu(fileName = "StoreSlotData", menuName = "ScriptableObjects/Data/StoreSlotData")]
    public class StoreUpgradeSlotData : ScriptableObject
    {
        [field: SerializeField] public string Label { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public int MaximumLevel { get; private set; }
    }
}