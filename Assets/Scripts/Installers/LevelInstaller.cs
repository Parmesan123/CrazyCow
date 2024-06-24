using Entities;
using Handlers;
using Level;
using Skills;
using UnityEngine;
using Wallet;
using Zenject;

namespace Installers
{
    public class LevelInstaller : MonoInstaller
    {
        private const string PLAYER_PATH = "Prefabs/Player/Player";

        [SerializeField] private Transform _playerSpawnPosition;
        [SerializeField] private Transform _cameraHandlerParent;
        [SerializeField] private MainLevelHandler _mainLevel;
        [SerializeField] private BonusLevelHandler _bonusLevel;
        [SerializeField] private CoinSpawner _coinSpawner;

        public override void InstallBindings()
        {
            BindCoinSpawner();
            BindFactories();
            BindPlayer();
            BindSpawnHandler();
            BindLevel();
            BindSkillProvider();
        }

        private void BindCoinSpawner()
        {
            Container
                .Bind<CoinSpawner>()
                .FromInstance(_coinSpawner)
                .AsSingle();
        }

        private void BindFactories()
        {
            Container
                .BindInterfacesAndSelfTo<BoxFactory>()
                .FromNew()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<VaseFactory>()
                .FromNew()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PortalFactory>()
                .FromNew()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PowerUpFactory>()
                .FromNew()
                .AsSingle();
        }

        private void BindPlayer()
        {
            PlayerBehavior playerPrefab = Resources.Load<PlayerBehavior>(PLAYER_PATH);
            PlayerBehavior playerInstance = Container.InstantiatePrefabForComponent<PlayerBehavior>(playerPrefab);
            playerInstance.transform.position = _playerSpawnPosition.position;

            Container
                .Bind<PlayerBehavior>()
                .FromInstance(playerInstance)
                .AsSingle()
                .NonLazy();
        }

        private void BindSpawnHandler()
        {
            Container
                .Bind<SpawnHandler>()
                .FromNew()
                .AsSingle();
        }

        private void BindLevel()
        {
            Container
                .Bind<BonusLevelHandler>()
                .FromInstance(_bonusLevel)
                .AsSingle();
        }

        private void BindSkillProvider()
        {
            Container
                .Bind<SkillProvider>()
                .FromNew()
                .AsSingle();
        }
    }
}