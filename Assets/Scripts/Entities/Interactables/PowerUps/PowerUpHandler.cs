using System.Collections.Generic;
using Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Entities
{
    public class PowerUpHandler : MonoBehaviour
    {
        [SerializeField] private PowerUpHandlerData _data;
    
        private const int BASE_POOL_SIZE = 10;
    
        private Dictionary<IPowerUp, PowerUpData> _powerUpToData;
        private Pool<PowerUpBehavior> _powerUpPool;
        private BoxFactory _boxFactory;

        [Inject]
        private void Construct(PowerUpFactory powerUpFactory, BoxFactory boxFactory)
        {
            _powerUpPool = new Pool<PowerUpBehavior>(BASE_POOL_SIZE, powerUpFactory, transform);

            _boxFactory = boxFactory;
        }

        private void Awake()
        {
            PowerUpData movementSpeedData = Resources.Load<PowerUpData>("SO/PowerUps/MovementSpeedPowerUp");
            PowerUpData destroySpeedData = Resources.Load<PowerUpData>("SO/PowerUps/DestroySpeedPowerUp");
            PowerUpData destroyRangeData = Resources.Load<PowerUpData>("SO/PowerUps/DestroyRangePowerUp");
        
            _powerUpToData = new Dictionary<IPowerUp, PowerUpData>()
            {
                { new MovementSpeedPowerUp(movementSpeedData.Time), movementSpeedData },
                { new DestroySpeedPowerUp(destroySpeedData.Time), destroySpeedData },
                { new DestroyRangePowerUp(destroyRangeData.Time), destroyRangeData }
            };
        
            _boxFactory.OnDestroyBoxEvent += SpawnPowerUp;
            foreach (Box spawnedBox in _boxFactory.SpawnedBoxes)
                spawnedBox.OnDestroyEvent += SpawnPowerUp;
        }

        private void OnDestroy()
        {
            _boxFactory.OnDestroyBoxEvent -= SpawnPowerUp;
        }

        private void SpawnPowerUp(IDestroyable box)
        {
            float value = Random.value;
        
            if (value > _data.PowerUpSpawnChance)
                return;

            Box convertableBox = box as Box;
            PowerUpBehavior freePowerUp = _powerUpPool.ObjectGetFreeOrCreate();
            freePowerUp.transform.position = convertableBox.transform.position;

            int randomIndex = Random.Range(0, _powerUpToData.Keys.Count);
            foreach (IPowerUp powerUp in _powerUpToData.Keys)
            {
                if (randomIndex == 0)
                {
                    freePowerUp.PowerUp = powerUp;
                    freePowerUp.Data = _powerUpToData[powerUp];
                    freePowerUp.Spawn();
                    return;
                }
                
                --randomIndex;
            }
        }
    }
}