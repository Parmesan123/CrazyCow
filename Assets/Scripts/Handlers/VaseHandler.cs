using System;
using System.Collections.Generic;
using InteractableObject;
using ModestTree;
using Services;
using UnityEngine;
using Zenject;

namespace Handlers
{
    public class VaseHandler : IDisposable, ISignalReceiver<SpawnVaseAroundCrateSignal>, ISignalReceiver<SpawnBoxAroundVaseSignal>, ISignalReceiver<DestroyBoxAroundVaseSignal>
    {
        private readonly Dictionary<Vase, List<Box>> _activeVases;
        private readonly VaseHandlerData _data;
        private readonly SignalBus _signalBus;

        private int _currentVasesOnField;

        [Inject]
        public VaseHandler(VaseHandlerData vaseData, SignalBus signalBus)
        {
            _activeVases = new Dictionary<Vase, List<Box>>();
            _data = vaseData;

            _signalBus = signalBus;
            _signalBus.RegisterUnique<SpawnVaseAroundCrateSignal>(this);
            _signalBus.RegisterUnique<SpawnBoxAroundVaseSignal>(this);
            _signalBus.RegisterUnique<DestroyBoxAroundVaseSignal>(this);
        }
        
        public void Dispose()
        {
            _signalBus.Unregister<SpawnVaseAroundCrateSignal>(this);
            _signalBus.Unregister<SpawnBoxAroundVaseSignal>(this);
            _signalBus.Unregister<DestroyBoxAroundVaseSignal>(this);
        }

        public void AddVase(Vase vase)
        {
            _activeVases.Add(vase, new List<Box>());
        }
        
        public void Receive(SpawnVaseAroundCrateSignal signal)
        {
            Vase vase = signal.Vase;
            
            Collider[] colliders = Physics.OverlapSphere(vase.transform.position, _data.SeekingRadius);
            foreach (Collider col in colliders)
            {
                if (!col.TryGetComponent(out Box crate)) 
                    continue;

                Debug.Log($"Added crate {crate.transform.position} to vase {vase.transform.position}");
                
                AddCrate(vase, crate);
            }
        }

        public void Receive(SpawnBoxAroundVaseSignal signal)
        {
            Box box = signal.Box;
            
            Collider[] colliders = Physics.OverlapSphere(box.transform.position, _data.SeekingRadius);
            foreach (Collider col in colliders) 
            {
                if (!col.TryGetComponent(out Vase vase)) 
                    continue;

                Debug.Log($"Added crate {box.transform.position} to vase {vase.transform.position}");
                AddCrate(vase, box); 
            }
        }

        public void Receive(DestroyBoxAroundVaseSignal signal)
        {
            Box box = signal.Box;
            
            RemoveBox(box);
        }

        private void AddCrate(Vase vase, Box box)
        {
            _activeVases[vase].Add(box);
        }

        private void RemoveBox(Box box)
        {
            foreach (Vase vase in _activeVases.Keys)
            {
                List<Box> boxes = _activeVases[vase];
                if (!boxes.Contains(box))
                    continue;
            
                boxes.Remove(box);
                if (!boxes.IsEmpty()) 
                    continue;
            
                vase.Destroy();
            }
        }
    }   
}