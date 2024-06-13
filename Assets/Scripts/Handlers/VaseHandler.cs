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
        private readonly Dictionary<Vase, List<Box>> _activeVases;
        private readonly VaseHandlerData _data;

        private readonly BoxFactory _boxFactory;
        private readonly VaseFactory _vaseFactory;

        private int _currentVasesOnField;

        [Inject]
        private VaseHandler(VaseHandlerData vaseData, BoxFactory boxFactory, VaseFactory vaseFactory)
        {
            _activeVases = new Dictionary<Vase, List<Box>>();
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
            foreach (Vase spawnedVase in _vaseFactory.SpawnedVases)
                spawnedVase.OnSpawn += VaseSpawned;
        }
        
        public void Dispose()
        {
            _boxFactory.OnSpawnBox -= BoxSpawned;
            _boxFactory.OnDestroyBox -= BoxRemoved;

            _vaseFactory.OnSpawnVase -= VaseSpawned;
        }
        
        private void VaseSpawned(ISpawnable spawnedVase)
        {
            if (spawnedVase is not Vase convertableVase)
                throw new Exception("Can't process callback in vase handler");
            
            _activeVases.Add(convertableVase, new List<Box>());
            
            Collider[] colliders = Physics.OverlapSphere(convertableVase.transform.position, _data.SeekingRadius);
            foreach (Collider col in colliders)
            {
                if (!col.TryGetComponent(out Box box)) 
                    continue;

                Debug.Log($"Added crate {box.transform.position} to vase {convertableVase.transform.position}");
                _activeVases[convertableVase].Add(box);
            }
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

                Debug.Log($"Added crate {convertableBox.transform.position} to vase {vase.transform.position}");
                _activeVases[vase].Add(convertableBox);
            }
        }
        
        private void BoxRemoved(IDestroyable box)
        {
            if (box is not Box convertableBox)
                throw new Exception("Remove request can't be processed in vase handler");
                
            List<Vase> removeVaseCollection = new List<Vase>();
            
            foreach (Vase vase in _activeVases.Keys)
            {
                List<Box> boxes = _activeVases[vase];
                if (!boxes.Contains(convertableBox))
                    continue;
            
                boxes.Remove(convertableBox);
                if (!boxes.IsEmpty()) 
                    continue;
            
                vase.Destroy();
                removeVaseCollection.Add(vase);
            }

            foreach (Vase nonActiveVase in removeVaseCollection)
            {
                if (!_activeVases.ContainsKey(nonActiveVase))
                    continue;
                
                _activeVases.Remove(nonActiveVase);
            }
        }
    }   
}