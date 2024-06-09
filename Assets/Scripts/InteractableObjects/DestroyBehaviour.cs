using System;
using System.Collections;
using UnityEngine;

namespace InteractableObject
{
	[SelectionBase]
	public abstract class DestroyBehaviour : MonoBehaviour, IDestroyable, ISpawnable
	{
		public event Action<DestroyBehaviour> OnDestroy;
		public event Action OnSpawn;
        
		[SerializeField] protected DestroyableData _data;

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

		public virtual void Destroy()
		{
			OnDestroy?.Invoke(this);
			gameObject.SetActive(false);
		}

		public virtual void Spawn()
		{
			OnSpawn?.Invoke();
			gameObject.SetActive(true);
		}
	}
}