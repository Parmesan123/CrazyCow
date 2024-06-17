﻿using System;
using System.Collections.Generic;
using Services;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class BoxFactory : MonoFactory<Box>, IDisposable
    {
        private const string BOX_PATH = "Prefabs/Box/Box";
        
        public event Action<ISpawnable> OnSpawnBoxEvent;
        public event Action<IDestroyable> OnDestroyBoxEvent;

        public IEnumerable<Box> SpawnedBoxes => _spawnedBoxes;
            
        private readonly Box _boxPrefab;
        private readonly List<Box> _spawnedBoxes;

        [Inject]
        public BoxFactory(DiContainer container) : base(container)
        {
            _boxPrefab = Resources.Load<Box>(BOX_PATH);

            _spawnedBoxes = new List<Box>();
        }

        public override Box CreateObject()
        {
            Box boxInstance = _container.InstantiatePrefabForComponent<Box>(_boxPrefab);
            boxInstance.gameObject.SetActive(false);
            
            boxInstance.OnSpawnEvent += OnSpawnBoxEvent;
            boxInstance.OnDestroyEvent += OnDestroyBoxEvent;

            _spawnedBoxes.Add(boxInstance);
            return boxInstance;
        }

        public void Dispose()
        {
            foreach (Box spawnedBox in _spawnedBoxes)
            {
                spawnedBox.OnSpawnEvent -= OnSpawnBoxEvent;
                spawnedBox.OnDestroyEvent -= OnDestroyBoxEvent;
            }
        }
    }
}