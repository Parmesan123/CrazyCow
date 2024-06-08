using NaughtyAttributes;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Data Installer", menuName = "SO/Data Installer", order = 0)]
public class DataInstaller : ScriptableObjectInstaller<DataInstaller>
{
    [SerializeField, Expandable] private SpawnData _spawnData; 
    
    public override void InstallBindings()
    {
        BindSpawnData();
    }

    private void BindSpawnData()
    {
        Container
            .Bind<SpawnData>()
            .FromInstance(_spawnData)
            .AsSingle();
    }
}