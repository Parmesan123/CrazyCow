using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SpawnHandler
{
    private const int INITIAL_POOL_SIZE = 30;
    
    private SpawnHandlerData _spawnHandlerData;
    private Transform _playerTransform;

    private Pool<Crate> _cratePool;
    private Pool<Vase> _vasePool;
    
    [Inject]
    public void Construct(CrateFactory crateFactory, VaseFactory vaseFactory, SpawnHandlerData spawnHandlerData, PlayerTest playerTest)
    {
        GameObject crateParent = new GameObject("Crates");
        _cratePool = new Pool<Crate>(INITIAL_POOL_SIZE, crateFactory, crateParent.transform);

        GameObject vaseParent = new GameObject("Vases");
        _vasePool = new Pool<Vase>(INITIAL_POOL_SIZE, vaseFactory, vaseParent.transform);
        
        _playerTransform = playerTest.transform;
        _spawnHandlerData = spawnHandlerData;
    }

    public T TrySpawnAndPlaceEntity<T>(BoxCollider levelCollider) where T : PoolableBehaviour
    {
        Vector3 randomPoint = CalculateValidPoint(_playerTransform, levelCollider, _spawnHandlerData.SpawnRadiusThreshold, 10);
        
        Crate crate = _cratePool.ObjectGetFreeOrCreate();
        if (crate is T convertableCrate)
        {
            convertableCrate.transform.position = randomPoint;
            convertableCrate.Enable();
            return convertableCrate;
        }

        Vase vase = _vasePool.ObjectGetFreeOrCreate();
        if (vase is T convertableVase)
        {
            convertableVase.transform.position = randomPoint;
            convertableVase.Enable();
            
            int i = Random.Range(_spawnHandlerData.VaseCratesMinMaxCount.x, _spawnHandlerData.VaseCratesMinMaxCount.y);
            for (; i < _spawnHandlerData.VaseCratesMinMaxCount.y; i++)
            {
                Crate freeCrate = _cratePool.ObjectGetFreeOrCreate();
        
                Vector3 cratePosition = CalculateValidPoint(convertableVase.transform, levelCollider, _spawnHandlerData.VaseRadiusThreshold, 0);
                freeCrate.transform.position = cratePosition;
                freeCrate.Enable();
            }
            
            return convertableVase;
        }
        
        Debug.Log("Should spawn portal");

        return null;
    }
    
    public bool InColliderBounds(BoxCollider boxCollider, Vector3 position)
    {
        return boxCollider.bounds.Contains(position);
    }

    private Vector3 CalculateValidPoint(Transform center, BoxCollider levelCollider, float radius, float offset)
    {
        for (int i = 0; i < 200; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
            Vector3 resultPosition = center.GetRandomPointOnCircle(radius) + randomOffset;
            
            Collider[] colliders = Physics.OverlapSphere(resultPosition, _spawnHandlerData.MinRangeBetweenObjects);
            if (colliders.Any(c => c.TryGetComponent(out PoolableBehaviour _)))
                continue;

            if (!InColliderBounds(levelCollider, resultPosition)) 
                continue;

            return resultPosition;
        }

        return Vector3.zero;
    }
}