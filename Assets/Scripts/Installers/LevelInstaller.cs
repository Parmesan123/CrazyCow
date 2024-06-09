using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    private const string PLAYER_PATH = "Prefabs/Player";
    
    [SerializeField] private Transform _playerSpawnPosition;
    [SerializeField] private LevelBehaviour _mainLevel;
    
    public override void InstallBindings()
    {
        BindGameElements();
        BindFactories();
        BindPlayer();
        BindSpawner();
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
    
    private void BindFactories()
    {
        Container
            .BindInterfacesAndSelfTo<CrateFactory>()
            .FromNew()
            .AsSingle();

        Container
            .BindInterfacesAndSelfTo<VaseFactory>()
            .FromNew()
            .AsSingle();
    }
    
    private void BindPlayer()
    {
        PlayerTest playerPrefab = Resources.Load<PlayerTest>(PLAYER_PATH);
        PlayerTest playerInstance = Container.InstantiatePrefabForComponent<PlayerTest>(playerPrefab);
        playerInstance.transform.position = _playerSpawnPosition.position;

        Container
            .Bind<PlayerTest>()
            .FromInstance(playerInstance)
            .AsSingle()
            .NonLazy();
    }

    private void BindSpawner()
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