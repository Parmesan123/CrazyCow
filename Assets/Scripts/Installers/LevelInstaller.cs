using Player;
using UI;
using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    private const string PLAYER_PATH = "Prefabs/Player/Player";
    
    [SerializeField] private Transform _playerSpawnPosition;
    [SerializeField] private LevelBehaviour _mainLevel;
    [SerializeField] private CoinSpawner _coinSpawner;
    
    public override void InstallBindings()
    {
        BindGameElements();
        BindCoinSpawner();
        BindFactories();
        BindPlayer();
        BindLevelSpawner();
        BindLevel();
        FinishBindings();
    }

    private void BindGameElements()
    {
        Container
            .Bind<VaseHandler>()
            .FromNew()
            .AsSingle();
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
    }
    
    private void BindPlayer()
    {
        PlayerMovement playerPrefab = Resources.Load<PlayerMovement>(PLAYER_PATH);
        PlayerMovement playerInstance = Container.InstantiatePrefabForComponent<PlayerMovement>(playerPrefab);
        playerInstance.transform.position = _playerSpawnPosition.position;

        Container
            .Bind<PlayerMovement>()
            .FromInstance(playerInstance)
            .AsSingle()
            .NonLazy();
    }

    private void BindLevelSpawner()
    {
        Container
            .Bind<SpawnHandler>()
            .FromNew()
            .AsSingle();
    }

    private void BindLevel()
    {
        Container
            .Bind<LevelBehaviour>()
            .FromInstance(_mainLevel)
            .AsSingle();
    }

    private void FinishBindings()
    {
        Destroy(_playerSpawnPosition.gameObject);
    }
}