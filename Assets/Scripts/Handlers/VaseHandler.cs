using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
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

        public Action<Box, Vase> OnVaseDestroyedFromBoxEvent;

        private int _currentVasesOnField;

        [Inject]
        private VaseHandler(VaseHandlerData vaseData, BoxFactory boxFactory, VaseFactory vaseFactory)
        {
            _activeVases = new List<Vase>();
            _data = vaseData;

            _boxFactory = boxFactory;
            _boxFactory.OnSpawnBoxEvent += BoxEventSpawned;
            foreach (Box spawnedBox in _boxFactory.SpawnedBoxes)
                spawnedBox.OnSpawnEvent += BoxEventSpawned;

            _vaseFactory = vaseFactory;
            _vaseFactory.OnSpawnVaseEvent += VaseEventSpawned;
            _vaseFactory.OnDestroyVaseEvent += VaseEventRemoved;
            foreach (Vase spawnedVase in _vaseFactory.SpawnedVases)
            {
                spawnedVase.OnSpawnEvent += VaseEventSpawned;
                spawnedVase.OnDestroyEvent += VaseEventRemoved;
            }
        }

        public void Dispose()
        {
            _boxFactory.OnSpawnBoxEvent -= BoxEventSpawned;

            _vaseFactory.OnSpawnVaseEvent -= VaseEventSpawned;
            _vaseFactory.OnDestroyVaseEvent -= VaseEventRemoved;
        }
        
        private void VaseEventSpawned(ISpawnable spawnedVase)
        {
            Vase convertableVase = spawnedVase as Vase;
            _activeVases.Add(convertableVase);
            
            Collider[] colliders = Physics.OverlapSphere(convertableVase.transform.position, _data.SeekingRadius);
            foreach (Collider col in colliders)
            {
                if (!col.TryGetComponent(out Box box)) 
                    continue;

                convertableVase.TryAddBox(box);
                box.OnDestroyEvent += BoxEventRemoved;
            }
        }
        
        private void VaseEventRemoved(IDestroyable vase)
        {
            Vase convertableVase = vase as Vase;
            
            _activeVases.Remove(convertableVase);
        }

        private void BoxEventSpawned(ISpawnable spawnedBox)
        {
            Box convertableBox = spawnedBox as Box;
            
            Collider[] colliders = Physics.OverlapSphere(convertableBox.transform.position, _data.SeekingRadius);
            foreach (Collider col in colliders) 
            {
                if (!col.TryGetComponent(out Vase vase)) 
                    continue;

                if (!vase.TryAddBox(convertableBox))
                    continue;
                
                convertableBox.OnDestroyEvent += BoxEventRemoved;
            }
        }
        
        private void BoxEventRemoved(IDestroyable box)
        {
            Box convertableBox = box as Box;
            convertableBox.OnDestroyEvent -= BoxEventRemoved;
                
            List<Vase> removeVaseCollection = _activeVases.Where(vase => vase.TryRemoveBox(convertableBox)).ToList();
            foreach (Vase nonActiveVase in removeVaseCollection)
            {
                nonActiveVase.Destroy();
                OnVaseDestroyedFromBoxEvent?.Invoke(convertableBox, nonActiveVase);
            }
        }
    }   
}