using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SpawnHandler : MonoBehaviour
{
    private SpawnData _spawnData;
    private Transform _playerTransform;
    private Pool<Crate> _cratePool;
    private Pool<Vase> _vasePool;
    private VaseHandler _vaseHandler;
    
    [Inject]
    public void Construct(CrateFactory crateFactory, VaseFactory vaseFactory, SpawnData spawnData, PlayerTest playerTest)
    {
        GameObject crateParent = new GameObject("Crates");
        _cratePool = new Pool<Crate>(spawnData.InitialCrateCount, crateFactory, crateParent.transform);

        GameObject vaseParent = new GameObject("Vases");
        _vasePool = new Pool<Vase>(spawnData.InitialVaseCount, vaseFactory, vaseParent.transform);
        _vaseHandler = new VaseHandler();
        
        _playerTransform = playerTest.transform;
        _spawnData = spawnData;

        for (int i = 0; i < _spawnData.InitialCrateCount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            Vector3 cratePosition = _playerTransform.CalculatePointOnCircle(_spawnData.BaseSpawnRadiusThreshold);
            SpawnAndPlaceCrate((cratePosition + randomOffset) * Mathf.Pow(-1, i));
        }

        for (int i = 0; i < _spawnData.InitialVaseCount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            Vector3 vasePosition = _playerTransform.CalculatePointOnCircle(_spawnData.BaseSpawnRadiusThreshold);
            SpawnAndPlaceVase((vasePosition + randomOffset) * Mathf.Pow(-1, i));
        }
        
        StartCoroutine(NextSpawnTick());
    }

    private IEnumerator NextSpawnTick()
    {
        yield return new WaitForSeconds(_spawnData.BaseSpawnTime);

        float tickResult = Random.Range(0f, 1f);
        if (tickResult <= _spawnData.CrateSpawnChance)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            Vector3 cratePosition = _playerTransform.CalculatePointOnCircle(_spawnData.BaseSpawnRadiusThreshold);

            float sideComponent = Random.Range(-1f, 1f);
            if (sideComponent > 0)
                SpawnAndPlaceCrate(cratePosition + randomOffset);
            else 
                SpawnAndPlaceCrate(-cratePosition + randomOffset);
        }

        if (tickResult <= _spawnData.VaseSpawnChance)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            Vector3 vasePosition = _playerTransform.CalculatePointOnCircle(_spawnData.BaseSpawnRadiusThreshold);
            
            float sideComponent = Random.Range(-1f, 1f);
            if (sideComponent > 0)
                SpawnAndPlaceVase(vasePosition + randomOffset);
            else 
                SpawnAndPlaceVase(-vasePosition + randomOffset);
        }
        
        yield return NextSpawnTick();
    }

    private void SpawnAndPlaceCrate(Vector3 position)
    {
        Crate freeCrate = _cratePool.ObjectGetFreeOrCreate();
        freeCrate.transform.position = position;
    }

    private void SpawnAndPlaceVase(Vector3 position)
    {
        Vase freeVase = _vasePool.ObjectGetFreeOrCreate();
        freeVase.transform.position = position;
        _vaseHandler.AddVase(freeVase);

        for (int i = 0; i < _spawnData.VaseCratesMinMaxCount.y; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
            Vector3 cratePosition = freeVase.transform.CalculatePointOnCircle(_spawnData.VaseRadiusThreshold);

            Crate freeCrate = _cratePool.ObjectGetFreeOrCreate();
            freeCrate.transform.position = cratePosition + randomOffset;
            freeCrate.OnDeath += () => _vaseHandler.RemoveCrateFrom(freeVase, freeCrate);
            _vaseHandler.AddCrateTo(freeVase, freeCrate);
        }
    }
}