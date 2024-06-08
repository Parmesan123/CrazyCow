using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Data", menuName = "SO/Spawn Data", order = 0)]
public class SpawnData : ScriptableObject
{
    [field: Header("Crate Info")]
    [field: SerializeField, Range(0, 1)] public float CrateSpawnChance { get; private set; }
    [field: SerializeField] public float MinRangeBetweenCrates { get; private set; }
    [field: SerializeField] public int InitialCrateCount { get; private set; }
    
    [field: Header("Vase Info")]
    [field: SerializeField, Range(0, 1)] public float VaseSpawnChance { get; private set; }
    [field: SerializeField] public float VaseRadiusThreshold { get; private set; }
    [field: SerializeField] public Vector2Int VaseCratesMinMaxCount { get; private set; }
    [field: SerializeField] public int InitialVaseCount { get; private set; }
    
    [field: Header("Base Values")]
    [field: SerializeField] public float BaseSpawnRadiusThreshold { get; private set; }
    [field: SerializeField] public float BaseSpawnTime { get; private set; }
}