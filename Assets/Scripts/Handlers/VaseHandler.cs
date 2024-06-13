using System;
using System.Collections.Generic;
using InteractableObject;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Handlers
{
    public class VaseHandler : IDisposable
    {
        private readonly List<Vase> _activeVases;
        private readonly VaseHandlerData _data;

        private readonly BoxFactory _boxFactory;
        private readonly VaseFactory _vaseFactory;

        private int _currentVasesOnField;

        [Inject]
        private VaseHandler(VaseHandlerData vaseData, BoxFactory boxFactory, VaseFactory vaseFactory)
        {
            _activeVases = new List<Vase>();
            _data = vaseData;

            _boxFactory = boxFactory;
            _boxFactory.OnSpawnBox += BoxSpawned;
            _boxFactory.OnDestroyBox += BoxRemoved;
            foreach (Box spawnedBox in _boxFactory.SpawnedBoxes)
            {
                spawnedBox.OnSpawn += BoxSpawned;
                spawnedBox.OnDestroy += BoxRemoved;
            }

            _vaseFactory = vaseFactory;
            _vaseFactory.OnSpawnVase += VaseSpawned;
            _vaseFactory.OnDestroyVase += VaseRemoved;
            foreach (Vase spawnedVase in _vaseFactory.SpawnedVases)
            {
                spawnedVase.OnSpawn += VaseSpawned;
                spawnedVase.OnDestroy += VaseRemoved;
            }
        }

        public void Dispose()
        {
            _boxFactory.OnSpawnBox -= BoxSpawned;
            _boxFactory.OnDestroyBox -= BoxRemoved;

            _vaseFactory.OnSpawnVase -= VaseSpawned;
            _vaseFactory.OnDestroyVase -= VaseRemoved;
        }
        
        private void VaseSpawned(ISpawnable spawnedVase)
        {
            if (spawnedVase is not Vase convertableVase)
                throw new Exception("Can't process callback in vase handler");
            
            _activeVases.Add(convertableVase);
            
            Collider[] colliders = Physics.OverlapSphere(convertableVase.transform.position, _data.SeekingRadius);
            foreach (Collider col in colliders)
            {
                if (!col.TryGetComponent(out Box box)) 
                    continue;

                Debug.Log($"Added crate {box.transform.position} to vase {convertableVase.transform.position}");
                convertableVase.TryAddBox(box);
            }
        }
        
        private void VaseRemoved(IDestroyable vase)
        {
            if (vase is not Vase convertableVase)
                throw new Exception("Remove request can't be processed in vase handler");

            if (!_activeVases.Contains(convertableVase))
                return;
            
            _activeVases.Remove(convertableVase);
            convertableVase.Destroy();
        }

        private void BoxSpawned(ISpawnable spawnedBox)
        {
            if (spawnedBox is not Box convertableBox)
                throw new Exception("Can't process callback in vase handler");
            
            Collider[] colliders = Physics.OverlapSphere(convertableBox.transform.position, _data.SeekingRadius);
            foreach (Collider col in colliders) 
            {
                if (!col.TryGetComponent(out Vase vase)) 
                    continue;

                if (!vase.TryAddBox(convertableBox))
                    continue;
                
                Debug.Log($"Added crate {convertableBox.transform.position} to vase {vase.transform.position}");
            }
        }
        
        private void BoxRemoved(IDestroyable box)
        {
            if (box is not Box convertableBox)
                throw new Exception("Remove request can't be processed in vase handler");
                
            List<Vase> removeVaseCollection = new List<Vase>();
            
            foreach (Vase vase in _activeVases)
            {
                if (!vase.TryRemoveBox(convertableBox))
                    continue;
                
                removeVaseCollection.Add(vase);
            }

            foreach (Vase nonActiveVase in removeVaseCollection)
            {
                if (!_activeVases.Contains(nonActiveVase))
                    continue;
                
                _activeVases.Remove(nonActiveVase);
                nonActiveVase.Destroy();
            }
        }
    }   
}