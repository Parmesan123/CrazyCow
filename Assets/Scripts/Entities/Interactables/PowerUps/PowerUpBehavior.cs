using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class PowerUpBehavior : InteractableBehavior, ISpawnable
    {
        public event Action<ISpawnable> OnSpawnEvent;
        public IPowerUp PowerUp;
        public PowerUpData Data;

        private PlayerBehavior _playerBehavior;

        private List<IPowerUp> _possiblePowerUps;

        [Inject]
        private void Construct(PlayerBehavior player)
        {
            _playerBehavior = player;
        }

        private void OnEnable()
        {
            GetComponentInChildren<Renderer>().material = Data.Material;
        }

        public void Spawn()
        {
            OnSpawnEvent?.Invoke(this);
            gameObject.SetActive(true);

            StartCoroutine(DespawnTimer());
            
            return;

            IEnumerator DespawnTimer()
            {
                float time = Data.Time;
                WaitForFixedUpdate timeDelay = new WaitForFixedUpdate();

                for (; time >= 0;)
                {
                    time -= Time.fixedDeltaTime;

                    yield return timeDelay;
                }
                
                Debug.Log("Test entity disappear log");

                gameObject.SetActive(false);
            }
        }
    
        protected override void Interact()
        {
            _playerBehavior.AddPowerUp(PowerUp);
            gameObject.SetActive(false);
        }
    }
}
