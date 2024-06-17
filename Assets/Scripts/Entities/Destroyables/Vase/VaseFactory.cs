using System;
using System.Collections.Generic;
using Services;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class VaseFactory : MonoFactory<Vase>, IDisposable
    {
        private const string VASE_PATH = "Prefabs/Vase/Vase";

        public event Action<ISpawnable> OnSpawnVaseEvent;
        public event Action<IDestroyable> OnDestroyVaseEvent;

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

            vaseInstance.OnSpawnEvent += OnSpawnVaseEvent;
            vaseInstance.OnDestroyEvent += OnDestroyVaseEvent;
        
            _spawnedVases.Add(vaseInstance);
            return vaseInstance;
        }

        public void Dispose()
        {
            foreach (Vase spawnedVase in _spawnedVases)
            {
                spawnedVase.OnSpawnEvent -= OnSpawnVaseEvent;
                spawnedVase.OnDestroyEvent -= OnDestroyVaseEvent;
            }
        }
    }
}