using System.Collections;
using Saving;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class PlayerBehavior : MonoBehaviour
    {
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
        [field: SerializeField] public BoxDestroyer PlayerBoxDestroyer { get; private set; }

        private PlayerData _playerData;
        
        [Inject]
        private void Construct(SaveHandler saveHandler)
        {
            _playerData = saveHandler.SaveData.PlayerData;
        }
        
        private void Awake()
        {
            PlayerMovement.CurrentSpeed = _playerData.MovementSpeed;
            PlayerBoxDestroyer.DestroyBonusTime = _playerData.DestroyBonusTime;
            PlayerBoxDestroyer.UpdateRange(_playerData.DestroyRange);
        }

        public void AddPowerUp(IPowerUp powerUp)
        {
            powerUp.Do(this);

            StartCoroutine(PowerUpTimer());
        
            return;

            IEnumerator PowerUpTimer()
            {
                float time = powerUp.Time;
                WaitForFixedUpdate timeDelay = new WaitForFixedUpdate();

                for (; time >= 0;)
                {
                    time -= Time.fixedDeltaTime;

                    yield return timeDelay;
                }

                powerUp.Undo(this);
            }
        }
    } 
}