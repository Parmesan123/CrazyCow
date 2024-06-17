using System.Collections;
using UnityEngine;

namespace Entities
{
    public class PlayerBehavior : MonoBehaviour
    { 
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
        [field: SerializeField] public BoxDestroyer PlayerBoxDestroyer { get; private set; }

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