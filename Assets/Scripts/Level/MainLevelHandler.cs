using System.Collections;
using System.Collections.Generic;
using Handlers;
using InteractableObject;
using Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Level
{
    public class MainLevelHandler : BaseLevelHandler
    {
        [SerializeField] private MainLevelData _mainLevelData;
        [SerializeField] private BoxCollider _boxCollider;

        private BoxFactory _boxFactory;
        private VaseFactory _vaseFactory;
        private PortalFactory _portalFactory;
        private float _currentPortalSpawnTime;
        private List<IDestroyable> _objectsOnLevel;

        private Coroutine _spawnRoutine;
        
        [Inject]
        protected void Construct(PauseHandler pauseHandler, SpawnHandler spawnHandler, PlayerMovement player, BoxFactory boxFactory, VaseFactory vaseFactory, PortalFactory portalFactory)
        {
            base.Construct(pauseHandler, spawnHandler, player);

            _objectsOnLevel = new List<IDestroyable>();

            _portalFactory = portalFactory;
            _portalFactory.OnPortalEnterEvent += PortalEntered;
            foreach (Portal spawnedPortal in _portalFactory.SpawnedPortals)
                spawnedPortal.OnEnter += PortalEntered;

            _boxFactory = boxFactory;
            _boxFactory.OnDestroyBoxEvent += DestroyEventEntity;
            foreach (Box spawnedBox in _boxFactory.SpawnedBoxes)
                spawnedBox.OnDestroyEvent += DestroyEventEntity;

            _vaseFactory = vaseFactory;
            _vaseFactory.OnDestroyVaseEvent += DestroyEventEntity;
            foreach (Vase spawnedVase in _vaseFactory.SpawnedVases)
                spawnedVase.OnDestroyEvent += DestroyEventEntity;
        }

        private void Awake()
        {
            StartLevel();
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _portalFactory.OnPortalEnterEvent -= PortalEntered;

            _boxFactory.OnDestroyBoxEvent -= DestroyEventEntity;

            _vaseFactory.OnDestroyVaseEvent -= DestroyEventEntity;
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
        
        private void DestroyEventEntity(IDestroyable destroyable)
        {
            _objectsOnLevel.Remove(destroyable);
        }
        
        private void PortalEntered()
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