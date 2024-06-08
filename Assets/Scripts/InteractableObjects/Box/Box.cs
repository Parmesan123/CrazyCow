using System;
using System.Collections;
using UnityEngine;

namespace InteractableObject
{
    public class Box : MonoBehaviour, IDestroyable
    {
        public event Action<IDestroyable> OnDestroyEvent; 
        
        [SerializeField] private BoxData _data;
        [SerializeField] private GameObject _model;

        private bool _isStopDestroy;
        
        public void StartDestroy()
        {
            StartCoroutine(Routine());
            return;

            IEnumerator Routine()
            {
                float timer = _data.TimeToDestroy;
                WaitForFixedUpdate wait = new WaitForFixedUpdate();
                _isStopDestroy = false;
                
                for (; timer >= 0;)
                {
                    timer -= Time.fixedDeltaTime;
                   
                    yield return wait;
                    
                    if (_isStopDestroy)
                        yield break;
                }
                
                Destroy();
            }
        }

        public void StopDestroy()
        {
            _isStopDestroy = true;
        }

        private void Destroy()
        {
            OnDestroyEvent.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}