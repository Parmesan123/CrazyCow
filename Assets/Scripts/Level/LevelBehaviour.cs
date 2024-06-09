﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelBehaviour : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private BoxCollider _boxCollider;
    
    private SpawnHandler _spawnHandler;
    private List<PoolableBehaviour> _objectsOnLevel;
    
    [Inject]
    public void Build(SpawnHandler spawnHandler)
    {
        _spawnHandler = spawnHandler;

        _objectsOnLevel = new List<PoolableBehaviour>();

        StartLevel();
    }

    private void StartLevel()
    {
        for (int i = 0; i < _levelData.CrateInitialCount; i++)
            OnEntitySpawnRequested<Crate>();

        for (int i = 0; i < _levelData.VaseInitialCount; i++)
            OnEntitySpawnRequested<Vase>();
        
        StartCoroutine(NextSpawnTick());
    }
    
    private IEnumerator NextSpawnTick()
    {
        yield return new WaitForSeconds(_levelData.SpawnTime);

        NextSpawnRequest();
        
        if (!_levelData.SpawnContinuously)
            yield break;
        
        yield return NextSpawnTick();
    }

    private void NextSpawnRequest()
    {
        if (_objectsOnLevel.Count > _levelData.ObjectsMaxCount)
            return;
        
        float tickResult = Random.Range(0f, 1f);
        if (tickResult <= _levelData.CrateSpawnChance)
            OnEntitySpawnRequested<Crate>();

        if (tickResult <= _levelData.VaseSpawnChance)
            OnEntitySpawnRequested<Vase>();

        if (tickResult <= _levelData.PortalSpawnChance)
            OnEntitySpawnRequested<Portal>();
    }

    private void OnEntitySpawnRequested<T>() where T: PoolableBehaviour
    {
        T entityInstance = _spawnHandler.TrySpawnAndPlaceEntity<T>(_boxCollider);
        _objectsOnLevel.Add(entityInstance);
        entityInstance.OnDestroy += OnEntityDestroyed;

        void OnEntityDestroyed()
        {
            _objectsOnLevel.Remove(entityInstance);
            entityInstance.OnDestroy -= OnEntityDestroyed;
        }    
    }
}