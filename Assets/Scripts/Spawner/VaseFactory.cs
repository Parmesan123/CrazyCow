using UnityEngine;
using Zenject;

public class VaseFactory : MonoFactory<Vase>
{
    private const string VASE_PATH = "Prefabs/Vase";

    private readonly Vase _vasePrefab;
    
    [Inject]
    public VaseFactory(DiContainer container) : base(container)
    {
        _vasePrefab = Resources.Load<Vase>(VASE_PATH);
    }

    public override Vase CreateObject()
    {
        Vase vaseInstance = _container.InstantiatePrefabForComponent<Vase>(_vasePrefab);

        return vaseInstance;
    }
}