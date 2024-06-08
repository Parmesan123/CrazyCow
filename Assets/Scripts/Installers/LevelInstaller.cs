using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    private const string PLAYER_PATH = "Prefabs/Player";
    
    [SerializeField] private Transform _playerSpawnPosition;
    
    public override void InstallBindings()
    {
        BindPlayer();
        BindFactories();
        BindSpawner();
        FinishBindings();
    }

    private void BindPlayer()
    {
        PlayerTest playerPrefab = Resources.Load<PlayerTest>(PLAYER_PATH);
        PlayerTest playerInstance = Container.InstantiatePrefabForComponent<PlayerTest>(playerPrefab);

        Container
            .Bind<PlayerTest>()
            .FromInstance(playerInstance)
            .AsSingle()
            .NonLazy();
    }

    private void BindFactories()
    {
        Container
            .BindInterfacesAndSelfTo<CrateFactory>()
            .FromNew()
            .AsSingle();
    }

    private void BindSpawner()
    {
        GameObject spawnHandler = new GameObject("SpawnHandler");

        Container.InstantiateComponent<SpawnHandler>(spawnHandler);
    }

    private void FinishBindings()
    {
        Destroy(_playerSpawnPosition.gameObject);
    }
}