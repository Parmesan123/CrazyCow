using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Data", menuName = "SO/Spawn Data", order = 0)]
public class SpawnHandlerData : ScriptableObject
{
    [field: SerializeField] public float SpawnRadiusThreshold { get; private set; }
    [field: SerializeField] public float MinRangeBetweenObjects { get; private set; }
    [field: SerializeField] public Vector2Int VaseCratesMinMaxCount { get; private set; }
    [field: SerializeField] public float VaseRadiusThreshold { get; private set; }
}
