using System;
using System.Collections.Generic;
using InteractableObject;
using Services;
using UnityEngine;
using Zenject;

public class PortalFactory : MonoFactory<Portal>, IDisposable
{
    private const string PORTAL_PREFAB_PATH = "Prefabs/Portal/Portal";

    public event Action OnPortalEnterEvent;
    
    public IEnumerable<Portal> SpawnedPortals => _spawnedPortals;
    
    private readonly Portal _portalPrefab;
    private readonly List<Portal> _spawnedPortals;
    
    public PortalFactory(DiContainer container) : base(container)
    {
        _portalPrefab = Resources.Load<Portal>(PORTAL_PREFAB_PATH);

        _spawnedPortals = new List<Portal>();
    }

    public override Portal CreateObject()
    {
        Portal portalInstance = _container.InstantiatePrefabForComponent<Portal>(_portalPrefab);
        portalInstance.gameObject.SetActive(false);

        portalInstance.OnEnter += OnPortalEnterEvent;

        _spawnedPortals.Add(portalInstance);
        return portalInstance;
    }

    public void Dispose()
    {
        foreach (Portal spawnedPortal in _spawnedPortals)
            spawnedPortal.OnEnter -= OnPortalEnterEvent;
    }
}