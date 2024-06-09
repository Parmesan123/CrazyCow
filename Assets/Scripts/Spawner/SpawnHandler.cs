using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SpawnHandler : MonoBehaviour
{
    private SpawnHandlerData _spawnHandlerData;
    private Transform _playerTransform;
    private Pool<Crate> _cratePool;
    private Pool<Vase> _vasePool;
    
    [Inject]
    public void Construct(CrateFactory crateFactory, VaseFactory vaseFactory, SpawnHandlerData spawnHandlerData, PlayerTest playerTest)
    {
        GameObject crateParent = new GameObject("Crates");
        _cratePool = new Pool<Crate>(spawnHandlerData.CrateInitialCount, crateFactory, crateParent.transform);

        GameObject vaseParent = new GameObject("Vases");
        _vasePool = new Pool<Vase>(spawnHandlerData.VaseInitialCount, vaseFactory, vaseParent.transform);
        
        _playerTransform = playerTest.transform;
        _spawnHandlerData = spawnHandlerData;

        for (int i = 0; i < _spawnHandlerData.CrateInitialCount; i++)
            SpawnAndPlaceCrate(_playerTransform, _spawnHandlerData.SpawnRadiusThreshold, 10f);

        for (int i = 0; i < _spawnHandlerData.VaseInitialCount; i++)
            SpawnAndPlaceVase();
        
        StartCoroutine(NextSpawnTick());
    }

    private IEnumerator NextSpawnTick()
    {
        yield return new WaitForSeconds(_spawnHandlerData.SpawnTime);

        float tickResult = Random.Range(0f, 1f);
        if (tickResult <= _spawnHandlerData.CrateSpawnChance)
            SpawnAndPlaceCrate(_playerTransform, _spawnHandlerData.SpawnRadiusThreshold, 10f);

        if (tickResult <= _spawnHandlerData.VaseSpawnChance) 
            SpawnAndPlaceVase();
        
        yield return NextSpawnTick();
    }

    private void SpawnAndPlaceCrate(Transform center, float radius, float offset)
    {
        Crate freeCrate = _cratePool.ObjectGetFreeOrCreate();
        while (true)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
            Vector3 cratePosition = center.GetRandomPointOnCircle(radius) + randomOffset;

            Collider[] colliders = Physics.OverlapSphere(cratePosition, _spawnHandlerData.MinRangeBetweenObjects);
            if (colliders.Length != 0)
                continue;
            
            freeCrate.transform.position = cratePosition;
            freeCrate.Enable();
            break;
        }
    }

    private void SpawnAndPlaceVase()
    {
        Vase freeVase = _vasePool.ObjectGetFreeOrCreate();
        while (true)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            Vector3 vasePosition = _playerTransform.GetRandomPointOnCircle(_spawnHandlerData.SpawnRadiusThreshold) + randomOffset;
                
            Collider[] colliders = Physics.OverlapSphere(vasePosition, _spawnHandlerData.MinRangeBetweenObjects);
            if (colliders.Length != 0)
                continue;

            freeVase.transform.position = vasePosition;
            freeVase.Enable();
            break;
        }

        int i = Random.Range(_spawnHandlerData.CratesMinMaxCount.x, _spawnHandlerData.CratesMinMaxCount.y);
        for (; i < _spawnHandlerData.CratesMinMaxCount.y; i++)
            SpawnAndPlaceCrate(freeVase.transform, _spawnHandlerData.VaseRadiusThreshold, 0);
    }
}