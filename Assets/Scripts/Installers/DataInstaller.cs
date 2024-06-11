using Handlers;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Data Installer", menuName = "SO/Data Installer", order = 0)]
public class DataInstaller : ScriptableObjectInstaller<DataInstaller>
{
    [SerializeField, Expandable] private SpawnHandlerData _spawnHandlerData;
    [SerializeField, Expandable] private VaseHandlerData _vaseHandlerData;
    
    public override void InstallBindings()
    {
        BindSpawnData();
        BindVaseData();
    }

    private void BindSpawnData()
    {
        Container
            .Bind<SpawnHandlerData>()
            .FromInstance(_spawnHandlerData)
            .AsSingle();
    }

    private void BindVaseData()
    {
        Container
            .Bind<VaseHandlerData>()
            .FromInstance(_vaseHandlerData)
            .AsSingle();
    }
}