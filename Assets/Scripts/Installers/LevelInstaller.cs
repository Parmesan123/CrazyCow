using Handlers;
using InteractableObject;
using Level;
using Player;
using UI;
using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    private const string PLAYER_PATH = "Prefabs/Player/Player";
    
    [SerializeField] private Transform _playerSpawnPosition;
    [SerializeField] private Transform _cameraHandlerParent;
    [SerializeField] private MainLevelHandler _mainLevel;
    [SerializeField] private CoinSpawner _coinSpawner;
    
    public override void InstallBindings()
    {
        BindCoinSpawner();
        BindFactories();
        BindPlayer();
        BindSpawnHandler();
        FinishBindings();
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

    private void BindSpawnHandler()
    {
        Container
            .Bind<SpawnHandler>()
            .FromNew()
            .AsSingle();
    }

    private void FinishBindings()
    {
        Destroy(_playerSpawnPosition.gameObject);
    }
}