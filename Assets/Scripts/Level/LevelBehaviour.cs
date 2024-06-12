using System;
using System.Collections;
using System.Collections.Generic;
using Handlers;
using InteractableObject;
using Player;
using Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Level
{
    public class LevelBehaviour : BaseLevelHandler, ISignalReceiver<DestroyEntitySignal>
    {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private BoxCollider _boxCollider;
        
        private float _currentPortalSpawnTime;
        private List<ISpawnable> _objectsOnLevel;
        private PlayerMovement _playerMovement;
        
        [Inject]
        protected override void Construct(SpawnHandler spawnHandler, PlayerMovement player, SignalBus signalBus, PauseHandler pauseHandler)
        {
            base.Construct(spawnHandler, player, signalBus, pauseHandler);

            _objectsOnLevel = new List<ISpawnable>();
        }

        private void Awake()
        {
            StartLevel();
        }

        public override void Unpause()
        {
            _signalBus.Register<DestroyEntitySignal>(this);

            StartCoroutine(NextSpawnTick());
        }

        public override void Pause()
        {
            _signalBus.Unregister<DestroyEntitySignal>(this);
            
            StopCoroutine(NextSpawnTick());
        }
        
        public void Receive(DestroyEntitySignal signal)
        {
            _objectsOnLevel.Remove(signal.Spawnable);
        }

        private void StartLevel()
        {
            for (int i = 0; i < _levelData.BoxInitialCount; i++)
                OnEntitySpawnRequested<Box>();

            for (int i = 0; i < _levelData.VaseInitialCount; i++)
                OnEntitySpawnRequested<Vase>();

            _currentPortalSpawnTime = _levelData.PortalSpawnTime;

            StartCoroutine(NextSpawnTick());
        }
        
        private IEnumerator NextSpawnTick()
        {
            yield return new WaitForSeconds(_levelData.SpawnTime);

            _currentPortalSpawnTime -= _levelData.PortalSpawnTime;
            NextSpawnRequest();

            if (!_levelData.SpawnContinuously)
                yield break;

            yield return NextSpawnTick();
        }

        private void NextSpawnRequest()
        {
            if (_objectsOnLevel.Count > _levelData.ObjectsMaxCount)
            {
                Debug.Log("Current objects exceeded maximum spawn number");
                return;
            }

            float tickResult = Random.Range(0f, 1f);
            if (tickResult <= _levelData.BoxSpawnChance)
                OnEntitySpawnRequested<Box>();

            if (tickResult <= _levelData.VaseSpawnChance)
                OnEntitySpawnRequested<Vase>();

            if (_currentPortalSpawnTime <= 0)
            {
                OnEntitySpawnRequested<Portal>();
                _currentPortalSpawnTime = _levelData.PortalSpawnTime;
            }
        }

        private void OnEntitySpawnRequested<T>() where T : ISpawnable
        {
            T entityInstance = _spawnHandler.TrySpawnAndPlaceEntity<T>(_boxCollider, _playerMovement.transform);
            if (entityInstance is IDestroyable _)
                _objectsOnLevel.Add(entityInstance);    
        }

        private void OnPortalSpawned()
        {
            
        }
    }
}