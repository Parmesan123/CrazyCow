using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/Data/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    [field: SerializeField] public float MovementSpeedValue { get; private set; }
    [field: SerializeField] public float DestroySpeedValue { get; private set; }
    [field: SerializeField] public float DestroyRangeValue { get; private set; }
}