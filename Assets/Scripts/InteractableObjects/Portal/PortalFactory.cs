using InteractableObject;
using Services;
using UnityEngine;
using Zenject;

public class PortalFactory : MonoFactory<Portal>
{
    private const string PORTAL_PREFAB_PATH = "Prefabs/Portal/Portal";

    private readonly Portal _portalPrefab;
    
    public PortalFactory(DiContainer container) : base(container)
    {
        _portalPrefab = Resources.Load<Portal>(PORTAL_PREFAB_PATH);
    }

    public override Portal CreateObject()
    {
        Portal portalInstance = _container.InstantiatePrefabForComponent<Portal>(_portalPrefab);
        portalInstance.gameObject.SetActive(false);

        return portalInstance;
    }
}