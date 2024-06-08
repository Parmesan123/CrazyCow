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
            int iter = 0;
            while (true)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                Vector3 cratePosition = _playerTransform.CalculatePointOnCircle(_spawnData.BaseSpawnRadiusThreshold);

                int sideComponent = Random.Range(1, 3);
                Vector3 resultPosition = (cratePosition + randomOffset) * Mathf.Pow(-1, sideComponent);

                if (++iter == 500)
                    break;
                Collider[] colliders = Physics.OverlapSphere(resultPosition, _spawnData.MinRangeBetweenCrates);
                if (colliders.Length != 0)
                    continue;
                
                SpawnAndPlaceCrate(resultPosition);
                break;
            }
        }

        for (int i = 0; i < _spawnData.InitialVaseCount; i++)
        {
            int iter = 0;
            while (true)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                Vector3 vasePosition = _playerTransform.CalculatePointOnCircle(_spawnData.BaseSpawnRadiusThreshold);

                int sideComponent = Random.Range(1, 3);
                Vector3 resultPosition = (vasePosition + randomOffset) * Mathf.Pow(-1, sideComponent);

                if (++iter == 500)
                    break;
                Collider[] colliders = Physics.OverlapSphere(resultPosition, _spawnData.MinRangeBetweenCrates);
                if (colliders.Length != 0)
                    continue;
                
                SpawnAndPlaceVase(resultPosition);
                break;
            }
        }
        
        StartCoroutine(NextSpawnTick());
    }

    private IEnumerator NextSpawnTick()
    {
        yield return new WaitForSeconds(_spawnData.BaseSpawnTime);

        float tickResult = Random.Range(0f, 1f);
        if (tickResult <= _spawnData.CrateSpawnChance)
        {
            int iter = 0;
            while (true)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                Vector3 cratePosition = _playerTransform.CalculatePointOnCircle(_spawnData.BaseSpawnRadiusThreshold);

                int sideComponent = Random.Range(1, 3);
                Vector3 resultPosition = (cratePosition + randomOffset) * Mathf.Pow(-1, sideComponent);

                if (++iter == 500)
                    break;
                Collider[] colliders = Physics.OverlapSphere(resultPosition, _spawnData.MinRangeBetweenCrates);
                if (colliders.Length != 0)
                    continue;
                
                SpawnAndPlaceCrate(resultPosition);
                break;
            }
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
            int iter = 0;
            while (true)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                Vector3 cratePosition = freeVase.transform.CalculatePointOnCircle(_spawnData.VaseRadiusThreshold);
                Vector3 resultPosition = cratePosition + randomOffset;

                if (++iter == 500)
                    break;
                Collider[] colliders = Physics.OverlapSphere(resultPosition, _spawnData.MinRangeBetweenCrates);
                if (colliders.Length != 0)
                    continue;
                
                Crate freeCrate = _cratePool.ObjectGetFreeOrCreate();
                freeCrate.transform.position = resultPosition;
                freeCrate.OnDeath += () => _vaseHandler.RemoveCrateFrom(freeVase, freeCrate);
                _vaseHandler.AddCrateTo(freeVase, freeCrate);
                break;
            }
        }
    }
}