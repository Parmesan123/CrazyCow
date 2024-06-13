using System.Collections;
using System.Collections.Generic;
using Handlers;
using InteractableObject;
using Player;
using Services;
using Signals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Level
{
    public class MainLevelHandler : BaseLevelHandler, ISignalReceiver<DestroyEntitySignal>, ISignalReceiver<PortalEnteredSignal>
    {
        [SerializeField] private MainLevelData _mainLevelData;
        [SerializeField] private BoxCollider _boxCollider;
        
        private float _currentPortalSpawnTime;
        private List<IDestroyable> _objectsOnLevel;
        
        [Inject]
        protected override void Construct(SpawnHandler spawnHandler, PlayerMovement player, SignalBus signalBus, PauseHandler pauseHandler)
        {
            base.Construct(spawnHandler, player, signalBus, pauseHandler);

            _objectsOnLevel = new List<IDestroyable>();
            
            _signalBus.RegisterUnique<DestroyEntitySignal>(this);
            _signalBus.RegisterUnique<PortalEnteredSignal>(this);
        }

        private void Awake()
        {
            StartLevel();
        }

        public override void Unpause()
        {
            _signalBus.RegisterUnique<DestroyEntitySignal>(this);
            _signalBus.RegisterUnique<PortalEnteredSignal>(this);

            StartCoroutine(NextSpawnTick());
        }

        public override void Pause()
        {
            _signalBus.Unregister<DestroyEntitySignal>(this);
            _signalBus.Unregister<PortalEnteredSignal>(this);
            
            StopCoroutine(NextSpawnTick());
        }
        
        public void Receive(DestroyEntitySignal signal)
        {
            _objectsOnLevel.Remove(signal.Entity);
        }
        
        public void Receive(PortalEnteredSignal signal)
        {
            Pause();
        }

        private void StartLevel()
        {
            for (int i = 0; i < _mainLevelData.BoxInitialCount; i++)
                OnEntitySpawnRequested<Box>();

            for (int i = 0; i < _mainLevelData.VaseInitialCount; i++)
                OnEntitySpawnRequested<Vase>();

            _currentPortalSpawnTime = _mainLevelData.PortalSpawnTime;

            StartCoroutine(NextSpawnTick());
        }
        
        private IEnumerator NextSpawnTick()
        {
            yield return new WaitForSeconds(_mainLevelData.SpawnTime);

            _currentPortalSpawnTime -= _mainLevelData.PortalSpawnTime;
            NextSpawnRequest();

            if (!_mainLevelData.SpawnContinuously)
                yield break;

            yield return NextSpawnTick();
        }

        private void NextSpawnRequest()
        {
            if (_objectsOnLevel.Count > _mainLevelData.ObjectsMaxCount)
            {
                Debug.Log("Current objects exceeded maximum spawn number");
                return;
            }

            float tickResult = Random.Range(0f, 1f);
            if (tickResult <= _mainLevelData.BoxSpawnChance)
                OnEntitySpawnRequested<Box>();

            if (tickResult <= _mainLevelData.VaseSpawnChance)
                OnEntitySpawnRequested<Vase>();

            if (_currentPortalSpawnTime <= 0)
            {
                OnEntitySpawnRequested<Portal>();
                _currentPortalSpawnTime = _mainLevelData.PortalSpawnTime;
            }
        }

        private void OnEntitySpawnRequested<T>() where T : ISpawnable
        {
            T entityInstance = _spawnHandler.TrySpawnAndPlaceEntity<T>(_boxCollider, _player.transform);
            if (entityInstance is IDestroyable convertable)
                _objectsOnLevel.Add(convertable);    
        }
    }
}