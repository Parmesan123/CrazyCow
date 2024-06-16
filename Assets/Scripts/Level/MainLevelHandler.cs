using System.Collections;
using System.Collections.Generic;
using Entities;
using Handlers;
using UI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Level
{
    public class MainLevelHandler : BaseLevelHandler
    {
        [SerializeField] private MainLevelData _mainLevelData;
        [SerializeField] private BoxCollider _boxCollider;

        private float _currentPortalSpawnTime;
        private BonusLevelHandler _bonusLevelHandler;
        private CoinSpawner _coinSpawner;
        private List<IDestroyable> _objectsOnLevel;

        private Coroutine _spawnRoutine;
        
        [Inject]
        protected void Construct(PauseHandler pauseHandler, SpawnHandler spawnHandler, PlayerMovement player, BonusLevelHandler bonusLevelHandler, CoinSpawner coinSpawner)
        {
            base.Construct(pauseHandler, spawnHandler, player);

            _objectsOnLevel = new List<IDestroyable>();

            _bonusLevelHandler = bonusLevelHandler;

            _coinSpawner = coinSpawner;
        }

        private void Awake()
        {
            _bonusLevelHandler.OnBonusLevelStarted += Pause;
            _bonusLevelHandler.OnBonusLevelEnded += Unpause;
            
            StartLevel();
        }
        
        public override void Unpause()
        {
            //TODO: rework
            _spawnRoutine = StartCoroutine(NextSpawnTick());
        }

        public override void Pause()
        {
            //TODO: rework
            StopCoroutine(_spawnRoutine);
        }

        private void StartLevel()
        {
            for (int i = 0; i < _mainLevelData.BoxInitialCount; i++)
                OnEntitySpawnRequested<Box>();
            
            for (int i = 0; i < _mainLevelData.VaseInitialCount; i++)
                OnEntitySpawnRequested<Vase>();

            _currentPortalSpawnTime = _mainLevelData.PortalSpawnTime;

            _spawnRoutine = StartCoroutine(NextSpawnTick());
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
            float tickResult = Random.Range(0f, 1f);
            if (_currentPortalSpawnTime <= 0)
            {
                OnEntitySpawnRequested<Portal>();
                _currentPortalSpawnTime = _mainLevelData.PortalSpawnTime;
            }
            
            if (_objectsOnLevel.Count > _mainLevelData.ObjectsMaxCount)
                return;

            if (tickResult <= _mainLevelData.BoxSpawnChance)
                OnEntitySpawnRequested<Box>();

            if (tickResult <= _mainLevelData.VaseSpawnChance)
                OnEntitySpawnRequested<Vase>();
        }

        private void OnEntitySpawnRequested<T>() where T : ISpawnable
        {
            T entityInstance = _spawnHandler.TrySpawnAndPlaceEntity<T>(_boxCollider, _player.transform);
            if (entityInstance is IDestroyable convertable)
            {
                convertable.OnDestroyEvent += DestroyEventEntity;
                _objectsOnLevel.Add(convertable);    
            }

            if (entityInstance is Box box)
                _coinSpawner.Register(box);

            if (entityInstance is Vase vase)
            {
                _coinSpawner.Register(vase);

                foreach (Box vaseBox in vase.Boxes)
                    _coinSpawner.Register(vaseBox);
            }
            
            return;
            
            void DestroyEventEntity(IDestroyable destroyable)
            {
                destroyable.OnDestroyEvent -= DestroyEventEntity;
                _objectsOnLevel.Remove(destroyable);
            }
        }
    }
}