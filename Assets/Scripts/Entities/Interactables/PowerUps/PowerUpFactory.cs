using Services;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class PowerUpFactory : MonoFactory<PowerUpBehavior>
    {
        private const string POWER_UP_PREFAB_PATH = "Prefabs/PowerUp/PowerUp";
    
        private readonly PowerUpBehavior _powerUpPrefab;
    
        public PowerUpFactory(DiContainer container) : base(container)
        {
            _powerUpPrefab = Resources.Load<PowerUpBehavior>(POWER_UP_PREFAB_PATH);
        }

        public override PowerUpBehavior CreateObject()
        {
            PowerUpBehavior powerUpInstance = _container.InstantiatePrefabForComponent<PowerUpBehavior>(_powerUpPrefab);
            powerUpInstance.gameObject.SetActive(false);

            return powerUpInstance;
        }
    }
}