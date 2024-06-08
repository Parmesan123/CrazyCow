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
        _cratePool = new Pool<Crate>(spawnData.CrateInitialCount, crateFactory, crateParent.transform);

        GameObject vaseParent = new GameObject("Vases");
        _vasePool = new Pool<Vase>(spawnData.VaseInitialCount, vaseFactory, vaseParent.transform);
        _vaseHandler = new VaseHandler();
        
        _playerTransform = playerTest.transform;
        _spawnData = spawnData;

        for (int i = 0; i < _spawnData.CrateInitialCount; i++)
            SpawnAndPlaceCrate(_playerTransform, _spawnData.SpawnRadiusThreshold, 10f, true);

        for (int i = 0; i < _spawnData.VaseInitialCount; i++)
            SpawnAndPlaceVase();
        
        StartCoroutine(NextSpawnTick());
    }

    private IEnumerator NextSpawnTick()
    {
        yield return new WaitForSeconds(_spawnData.SpawnTime);

        float tickResult = Random.Range(0f, 1f);
        if (tickResult <= _spawnData.CrateSpawnChance)
            SpawnAndPlaceCrate(_playerTransform, _spawnData.SpawnRadiusThreshold, 10f, true);

        if (tickResult <= _spawnData.VaseSpawnChance) 
            SpawnAndPlaceVase();
        
        yield return NextSpawnTick();
    }

    private Crate SpawnAndPlaceCrate(Transform center, float radius, float offset, bool shouldRandomize)
    {
        Crate freeCrate = _cratePool.ObjectGetFreeOrCreate();
        while (true)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
            Vector3 cratePosition = center.CalculatePointOnCircle(radius);

            Vector3 resultPosition = cratePosition + randomOffset;
            if (shouldRandomize)
            {
                int sideComponent = Random.Range(1, 3);
                resultPosition *= Mathf.Pow(-1, sideComponent);
            }

            Collider[] colliders = Physics.OverlapSphere(resultPosition, _spawnData.MinRangeBetweenCrates);
            if (colliders.Length != 0)
                continue;
            
            colliders = Physics.OverlapSphere(resultPosition, _spawnData.VaseRadiusThreshold);
            foreach (Collider col in colliders)
            {
                col.TryGetComponent(out Vase vase);
                freeCrate.OnDeath += () => _vaseHandler.RemoveCrateFrom(vase, freeCrate);
                _vaseHandler.AddCrateTo(vase, freeCrate);
            }
            
            freeCrate.transform.position = resultPosition;
            break;
        }

        return freeCrate;
    }

    private void SpawnAndPlaceVase()
    {
        Vase freeVase = _vasePool.ObjectGetFreeOrCreate();
        _vaseHandler.AddVase(freeVase);
        while (true)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            Vector3 vasePosition = _playerTransform.CalculatePointOnCircle(_spawnData.SpawnRadiusThreshold);
            
            int sideComponent = Random.Range(1, 3);
            Vector3 resultPosition = (vasePosition + randomOffset) * Mathf.Pow(-1, sideComponent);
                
            Collider[] colliders = Physics.OverlapSphere(resultPosition, _spawnData.MinRangeBetweenCrates);
            if (colliders.Length != 0)
                continue;

            colliders = Physics.OverlapSphere(resultPosition, _spawnData.VaseRadiusThreshold);
            foreach (Collider col in colliders)
            {
                col.TryGetComponent(out Crate crate);
                crate.OnDeath += () => _vaseHandler.RemoveCrateFrom(freeVase, crate);
                _vaseHandler.AddCrateTo(freeVase, crate);
            }
                    
            freeVase.transform.position = resultPosition;
            break;
        }

        for (int i = 0; i < _spawnData.CratesMinMaxCount.y; i++)
        {
            Crate freeCrate = SpawnAndPlaceCrate(freeVase.transform, _spawnData.VaseRadiusThreshold, 0.5f, false);
            freeCrate.OnDeath += () => _vaseHandler.RemoveCrateFrom(freeVase, freeCrate);
            _vaseHandler.AddCrateTo(freeVase, freeCrate);
        }
    }
}