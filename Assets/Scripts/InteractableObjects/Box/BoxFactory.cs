using InteractableObject;
using UI;
using UnityEngine;
using Zenject;

public class BoxFactory : MonoFactory<Box>
{
    private const string BOX_PATH = "Prefabs/Box/Box";

    private readonly Box _cratePrefab;
    private readonly VaseHandler _vaseHandler;
    
    [Inject]
    public BoxFactory(DiContainer container, VaseHandler vaseHandler) : base(container)
    {
        _cratePrefab = Resources.Load<Box>(BOX_PATH);

        _vaseHandler = vaseHandler;
    }

    public override Box CreateObject()
    {
        Box crateInstance = _container.InstantiatePrefabForComponent<Box>(_cratePrefab);
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

        void OnCrateDespawn(DestroyBehaviour destroyInstance)
        {
            _vaseHandler.RemoveCrate(crateInstance);
            crateInstance.OnDestroy -= OnCrateDespawn;
        }
    }
}