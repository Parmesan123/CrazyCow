﻿using System;
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
            if (spawnedVase is not Vase convertableVase)
                throw new Exception("Can't process callback in vase handler");
            
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
            if (vase is not Vase convertableVase)
                throw new Exception("Remove request can't be processed in vase handler");
            
            _activeVases.Remove(convertableVase);
            convertableVase.Destroy();
        }

        private void BoxEventSpawned(ISpawnable spawnedBox)
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
                
                convertableBox.OnDestroyEvent += BoxEventRemoved;
            }
        }
        
        private void BoxEventRemoved(IDestroyable box)
        {
            box.OnDestroyEvent -= BoxEventRemoved;
            if (box is not Box convertableBox)
                throw new Exception("Remove request can't be processed in vase handler");
                
            List<Vase> removeVaseCollection = _activeVases.Where(vase => vase.TryRemoveBox(convertableBox)).ToList();
            foreach (Vase nonActiveVase in removeVaseCollection)
            {
                _activeVases.Remove(nonActiveVase);
                nonActiveVase.Destroy();
            }
        }
    }   
}