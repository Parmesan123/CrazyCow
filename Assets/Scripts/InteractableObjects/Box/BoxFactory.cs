using Handlers;
using Services;
using UnityEngine;
using Zenject;

namespace InteractableObject
{
    public class BoxFactory : MonoFactory<Box>
    {
        private const string BOX_PATH = "Prefabs/Box/Box";

        private readonly Box _boxPrefab;

        [Inject]
        public BoxFactory(DiContainer container) : base(container)
        {
            _boxPrefab = Resources.Load<Box>(BOX_PATH);
        }

        public override Box CreateObject()
        {
            Box boxInstance = _container.InstantiatePrefabForComponent<Box>(_boxPrefab);
            boxInstance.gameObject.SetActive(false);

            return boxInstance;
        }
    }
}