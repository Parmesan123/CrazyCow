using System.Linq;
using InteractableObject;
using Player;
using UI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SpawnHandler
{
    private const int INITIAL_POOL_SIZE = 30;
    
    private SpawnHandlerData _spawnHandlerData;
    private Transform _playerTransform;
    private CoinSpawner _coinSpawner;
    
    private Pool<Box> _cratePool;
    private Pool<Vase> _vasePool;
    
    [Inject]
    public void Construct(BoxFactory boxFactory, VaseFactory vaseFactory, SpawnHandlerData spawnHandlerData, 
        PlayerMovement playerTest, CoinSpawner coinSpawner)
    {
        GameObject crateParent = new GameObject("Boxes");
        _cratePool = new Pool<Box>(INITIAL_POOL_SIZE, boxFactory, crateParent.transform);

        GameObject vaseParent = new GameObject("Vases");
        _vasePool = new Pool<Vase>(INITIAL_POOL_SIZE, vaseFactory, vaseParent.transform);

        _coinSpawner = coinSpawner;
        _playerTransform = playerTest.transform;
        _spawnHandlerData = spawnHandlerData;
    }

    public T TrySpawnAndPlaceEntity<T>(BoxCollider levelCollider) where T : DestroyBehaviour
    {
        Vector3 randomPoint = CalculateValidPoint(_playerTransform, levelCollider, _spawnHandlerData.SpawnRadiusThreshold, 10);
        
        Box crate = _cratePool.ObjectGetFreeOrCreate();
        if (crate is T convertableCrate)
        {
            convertableCrate.transform.position = randomPoint;
            convertableCrate.Spawn();
            _coinSpawner.Register(crate);
            return convertableCrate;
        }

        Vase vase = _vasePool.ObjectGetFreeOrCreate();
        if (vase is T convertableVase)
        {
            convertableVase.transform.position = randomPoint;
            convertableVase.Spawn();
            _coinSpawner.Register(vase);
            
            int i = Random.Range(_spawnHandlerData.VaseCratesMinMaxCount.x, _spawnHandlerData.VaseCratesMinMaxCount.y);
            for (; i < _spawnHandlerData.VaseCratesMinMaxCount.y; i++)
            {
                Box freeCrate = _cratePool.ObjectGetFreeOrCreate();
        
                Vector3 cratePosition = CalculateValidPoint(convertableVase.transform, levelCollider, _spawnHandlerData.VaseRadiusThreshold, 0);
                freeCrate.transform.position = cratePosition;
                freeCrate.Spawn();
                _coinSpawner.Register(freeCrate);
            }
            
            return convertableVase;
        }
        
        Debug.Log("Should spawn portal");

        return null;
    }
    
    private bool InColliderBounds(BoxCollider boxCollider, Vector3 position)
    {
        Vector2 minBounds = new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.z);
        Vector2 maxBounds = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.max.z);

        Vector2 convertPosition = new Vector2(position.x, position.z);

        if (convertPosition.x < minBounds.x || convertPosition.x > maxBounds.x)
            return false;

        if (convertPosition.y < minBounds.y || convertPosition.y > maxBounds.y)
            return false;
        
        return true;
    }

    private Vector3 CalculateValidPoint(Transform center, BoxCollider levelCollider, float radius, float offset)
    {
        for (int i = 0; i < 800; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
            Vector3 resultPosition = center.GetRandomPointOnCircle(radius) + randomOffset;
            
            Collider[] colliders = Physics.OverlapSphere(resultPosition, _spawnHandlerData.MinRangeBetweenObjects);
            if (colliders.Any(c => c.TryGetComponent(out DestroyBehaviour _)))
                continue;

            if (!InColliderBounds(levelCollider, resultPosition)) 
                continue;

            return resultPosition;
        }

        return Vector3.zero;
    }
}