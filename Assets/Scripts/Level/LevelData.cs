using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "SO/Level Data", order = 0)]
public class LevelData : ScriptableObject
{
    [field: Header("Crate Info")]
    [field: SerializeField, Range(0, 1)] public float CrateSpawnChance { get; private set; }
    [field: SerializeField] public int CrateInitialCount { get; private set; }
    
    [field: Header("Vase Info")]
    [field: SerializeField, Range(0, 1)] public float VaseSpawnChance { get; private set; }
    [field: SerializeField] public int VaseInitialCount { get; private set; }
    
    [field: Header("Portal Info")]
    [field: SerializeField, Range(0, 1)] public float PortalSpawnChance { get; private set; }
    
    [field: Header("Base Info")]
    [field: SerializeField] public int ObjectsMaxCount { get; private set; }
    [field: SerializeField] public bool SpawnContinuously { get; private set; }
    [field: SerializeField] public float SpawnTime { get; private set; }
}