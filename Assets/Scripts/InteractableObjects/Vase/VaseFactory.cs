using System;
using System.Collections.Generic;
using Services;
using UnityEngine;
using Zenject;

namespace InteractableObject
{
    public class VaseFactory : MonoFactory<Vase>, IDisposable
    {
        private const string VASE_PATH = "Prefabs/Vase/Vase";

        public event Action<ISpawnable> OnSpawnVase;
        public event Action<IDestroyable> OnDestroyVase;

        public IEnumerable<Vase> SpawnedVases => _spawnedVases;
        
        private readonly Vase _vasePrefab;
        private readonly List<Vase> _spawnedVases;
    
        [Inject]
        public VaseFactory(DiContainer container) : base(container)
        {
            _vasePrefab = Resources.Load<Vase>(VASE_PATH);

            _spawnedVases = new List<Vase>();
        }

        public override Vase CreateObject()
        {
            Vase vaseInstance = _container.InstantiatePrefabForComponent<Vase>(_vasePrefab);
            vaseInstance.gameObject.SetActive(false);

            vaseInstance.OnSpawn += OnSpawnVase;
            vaseInstance.OnDestroy += OnDestroyVase;
        
            _spawnedVases.Add(vaseInstance);
            return vaseInstance;
        }

        public void Dispose()
        {
            foreach (Vase spawnedVase in _spawnedVases)
            {
                spawnedVase.OnSpawn -= OnSpawnVase;
                spawnedVase.OnDestroy -= OnDestroyVase;
            }
        }
    }
}