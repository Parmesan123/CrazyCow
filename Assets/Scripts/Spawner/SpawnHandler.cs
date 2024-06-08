using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SpawnHandler : MonoBehaviour
{
    private Pool<Crate> _pool;
    private SpawnData _spawnData;
    private Transform _playerTransform;
    
    [Inject]
    public void Construct(CrateFactory crateFactory, SpawnData spawnData, PlayerTest playerTest)
    {
        GameObject crateParent = new GameObject("Crates");
        _pool = new Pool<Crate>(spawnData.InitialCrateCount, crateFactory, crateParent.transform);
        
        _playerTransform = playerTest.transform;
        _spawnData = spawnData;
        
        for (int i = 0; i < _spawnData.InitialCrateCount; i++)
            SpawnAndPlaceCrate();
        
        StartCoroutine(SpawnCrateAroundPlayer());
    }

    private IEnumerator SpawnCrateAroundPlayer()
    {
        yield return new WaitForSeconds(_spawnData.BaseSpawnTime);

        float tickResult = Random.Range(0f, 1f);
        if (tickResult > _spawnData.CrateSpawnChance)
            yield return SpawnCrateAroundPlayer();

        Debug.Log("Spawned!");
        SpawnAndPlaceCrate();
        yield return SpawnCrateAroundPlayer();
    }

    private void SpawnAndPlaceCrate()
    {
        float xCurrentMin = _playerTransform.position.x - _spawnData.BaseSpawnRadiusThreshold;
        float xCurrentMax = _playerTransform.position.x + _spawnData.BaseSpawnRadiusThreshold;
        
        float xResult = Random.Range(xCurrentMin, xCurrentMax);
        float yComponent = Mathf.Sqrt(Mathf.Pow(_spawnData.BaseSpawnRadiusThreshold, 2) - Mathf.Pow(xResult, 2));
        float yResult = yComponent + _playerTransform.position.y;

        Crate freeCrate = _pool.ObjectGetFreeOrCreate();
        Vector2 randomOffset = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        freeCrate.transform.position = new Vector2(xResult, yResult) + randomOffset;
    }
}