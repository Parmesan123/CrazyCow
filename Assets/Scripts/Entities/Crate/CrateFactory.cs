using UnityEngine;
using Zenject;

public class CrateFactory : MonoFactory<Crate>
{
    private const string CRATE_PATH = "Prefabs/Crate";

    private readonly Crate _cratePrefab;
    private readonly VaseHandler _vaseHandler;
    
    [Inject]
    public CrateFactory(DiContainer container, VaseHandler vaseHandler) : base(container)
    {
        _cratePrefab = Resources.Load<Crate>(CRATE_PATH);

        _vaseHandler = vaseHandler;
    }

    public override Crate CreateObject()
    {
        Crate crateInstance = _container.InstantiatePrefabForComponent<Crate>(_cratePrefab);
        crateInstance.gameObject.SetActive(false);
        crateInstance.OnSpawn += OnCrateSpawn;

        return crateInstance;
        
        void OnCrateSpawn()
        {
            Collider[] colliders = Physics.OverlapSphere(crateInstance.transform.position, _vaseHandler.Data.SeekingRadius);
            foreach (Collider col in colliders)
            {
                if (!col.TryGetComponent(out Vase vase)) 
                    continue;

                Debug.Log($"Added crate {crateInstance.transform.position} to vase {vase.transform.position}");
                
                _vaseHandler.AddCrate(vase, crateInstance);
                crateInstance.OnDestroy += OnCrateDespawn;
            }
        }

        void OnCrateDespawn()
        {
            _vaseHandler.RemoveCrate(crateInstance);
            crateInstance.OnDestroy -= OnCrateDespawn;
        }
    }
}