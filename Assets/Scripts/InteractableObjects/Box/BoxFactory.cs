using Services;
using UnityEngine;
using Zenject;

namespace InteractableObject
{
    public class BoxFactory : MonoFactory<Box>
    {
        private const string BOX_PATH = "Prefabs/Box/Box";

        private readonly Box _boxPrefab;
        private readonly VaseHandler _vaseHandler;
    
        [Inject]
        public BoxFactory(DiContainer container, VaseHandler vaseHandler) : base(container)
        {
            _boxPrefab = Resources.Load<Box>(BOX_PATH);

            _vaseHandler = vaseHandler;
        }

        public override Box CreateObject()
        {
            Box boxInstance = _container.InstantiatePrefabForComponent<Box>(_boxPrefab);
            boxInstance.gameObject.SetActive(false);
            boxInstance.OnSpawn += OnCrateSpawn;

            return boxInstance;
        
            void OnCrateSpawn()
            {
                Collider[] colliders = Physics.OverlapSphere(boxInstance.transform.position, _vaseHandler.Data.SeekingRadius);
                foreach (Collider col in colliders)
                {
                    if (!col.TryGetComponent(out Vase vase)) 
                        continue;

                    Debug.Log($"Added crate {boxInstance.transform.position} to vase {vase.transform.position}");
                
                    _vaseHandler.AddCrate(vase, boxInstance);
                    boxInstance.OnDestroy += OnCrateDespawn;
                }
            }

            void OnCrateDespawn(DestroyBehaviour destroyInstance)
            {
                _vaseHandler.RemoveBox(boxInstance);
                boxInstance.OnDestroy -= OnCrateDespawn;
            }
        }
    }
}