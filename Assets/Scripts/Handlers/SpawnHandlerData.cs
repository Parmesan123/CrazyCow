using NaughtyAttributes;
using UnityEngine;

namespace Spawner
{
    [CreateAssetMenu(fileName = "SpawnData", menuName = "ScriptableObjects/Data/SpawnData")]
    public class SpawnHandlerData : ScriptableObject
    {
        [SerializeField, MinMaxSlider(1, 6)] private Vector2Int _boxesWithVaseCount;
        
        [field: SerializeField] public float SpawnRadiusThreshold { get; private set; }
        [field: SerializeField] public float MinRangeBetweenObjects { get; private set; }
        [field: SerializeField] public float VaseRadiusThreshold { get; private set; }
        
        public int BoxesWithVaseCount => Random.Range(_boxesWithVaseCount.x, _boxesWithVaseCount.y + 1);
    }
}