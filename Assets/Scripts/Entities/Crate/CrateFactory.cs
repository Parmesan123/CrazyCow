using UnityEngine;
using Zenject;

public class CrateFactory : MonoFactory<Crate>
{
    private const string CRATE_PATH = "Prefabs/Crate";

    private readonly Crate _cratePrefab;
    
    [Inject]
    public CrateFactory(DiContainer container) : base(container)
    {
        _cratePrefab = Resources.Load<Crate>(CRATE_PATH);
    }

    public override Crate CreateObject()
    {
        Crate crateInstance = _container.InstantiatePrefabForComponent<Crate>(_cratePrefab);

        return crateInstance;
    }
}