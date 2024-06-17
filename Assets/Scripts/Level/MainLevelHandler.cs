using System.Collections;
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

        private float ObjectsCount => _boxesOnField.Count + _vasesOnField.Count;
        private float _currentPortalSpawnTime;
        
        private BonusLevelHandler _bonusLevelHandler;

        private Coroutine _spawnRoutine;
        
        [Inject]
        protected void Construct(SpawnHandler spawnHandler, CoinSpawner coinSpawner, PlayerMovement player, BonusLevelHandler bonusLevelHandler)
        {
            base.Construct(spawnHandler, coinSpawner, player);

            _bonusLevelHandler = bonusLevelHandler;
        }

        protected override void Awake()
        {
            base.Awake();
            
            _bonusLevelHandler.OnBonusLevelStarted += Pause;
            _bonusLevelHandler.OnBonusLevelEnded += Unpause;
            
            StartLevel();
        }
        
        private void Unpause()
        {
            _spawnRoutine = StartCoroutine(NextSpawnTick());
        }

        private void Pause()
        {
            StopCoroutine(_spawnRoutine);
        }

        private void StartLevel()
        {
            for (int i = 0; i < _mainLevelData.BoxInitialCount; i++)
            {
                Box box = EntitySpawn<Box>(_player.transform);
                _coinSpawner.Register(box);
            }

            for (int i = 0; i < _mainLevelData.VaseInitialCount; i++)
            {
                Vase vase = EntitySpawn<Vase>(_player.transform);
                _coinSpawner.Register(vase);
                
                foreach (Box box in vase.Boxes)
                    _coinSpawner.Register(box);
            }

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
                EntitySpawn<Portal>();
                _currentPortalSpawnTime = _mainLevelData.PortalSpawnTime;
            }
            
            if (ObjectsCount > _mainLevelData.ObjectsMaxCount)
                return;

            if (tickResult <= _mainLevelData.BoxSpawnChance)
            {
                Box box = EntitySpawn<Box>(_player.transform);
                _coinSpawner.Register(box);
            }

            if (tickResult <= _mainLevelData.VaseSpawnChance)
            {
                Vase vase = EntitySpawn<Vase>(_player.transform);
                _coinSpawner.Register(vase);
                
                foreach (Box box in vase.Boxes)
                    _coinSpawner.Register(box);
            }
        }
    }
}