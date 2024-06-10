using Services;
using UnityEngine;
using Zenject;

namespace InteractableObject
{
    public class VaseFactory : MonoFactory<Vase>
    {
        private const string VASE_PATH = "Prefabs/Vase/Vase";

        private readonly Vase _vasePrefab;
        private readonly VaseHandler _vaseHandler;
    
        [Inject]
        public VaseFactory(DiContainer container, VaseHandler vaseHandler) : base(container)
        {
            _vasePrefab = Resources.Load<Vase>(VASE_PATH);

            _vaseHandler = vaseHandler;
        }

        public override Vase CreateObject()
        {
            Vase vaseInstance = _container.InstantiatePrefabForComponent<Vase>(_vasePrefab);
            vaseInstance.gameObject.SetActive(false);
            vaseInstance.OnSpawn += OnVaseSpawn;
        
            _vaseHandler.AddVase(vaseInstance);
        
            return vaseInstance;

            void OnVaseSpawn()
            {
                Collider[] colliders = Physics.OverlapSphere(vaseInstance.transform.position, _vaseHandler.Data.SeekingRadius);
                foreach (Collider col in colliders)
                {
                    if (!col.TryGetComponent(out Box crate)) 
                        continue;

                    Debug.Log($"Added crate {crate.transform.position} to vase {vaseInstance.transform.position}");
                
                    _vaseHandler.AddCrate(vaseInstance, crate);
                }
            }
        }
    }
}