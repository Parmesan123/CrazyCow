using NaughtyAttributes;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Data/LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        [field: Header("Crate Info")]
        [field: SerializeField, Range(0, 1)] public float BoxSpawnChance { get; private set; }
        [field: SerializeField] public int BoxInitialCount { get; private set; }
    
        [field: Header("Vase Info")]
        [field: SerializeField, Range(0, 1)] public float VaseSpawnChance { get; private set; }
        [field: SerializeField] public int VaseInitialCount { get; private set; }
    
        [field: Header("Portal Info")]
        [SerializeField, MinMaxSlider(20f, 40f)] private Vector2 _portalSpawnTime;
        public float PortalSpawnTime => Random.Range(_portalSpawnTime.x, _portalSpawnTime.y);
    
        [field: Header("Additional Level Info")]
        [field: SerializeField] public int AddLevelBoxCount { get; private set; }
        [field: SerializeField] public int AddLevelVaseCount { get; private set; }
        
        [field: Header("Base Info")]
        [field: SerializeField] public int ObjectsMaxCount { get; private set; }
        [field: SerializeField] public bool SpawnContinuously { get; private set; }
        [field: SerializeField] public float SpawnTime { get; private set; }
    }
}