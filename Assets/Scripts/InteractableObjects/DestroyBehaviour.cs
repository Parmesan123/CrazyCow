using System;
using System.Collections;
using UnityEngine;

namespace InteractableObject
{
	[SelectionBase]
	public abstract class DestroyBehaviour : MonoBehaviour, ISpawnable, IDestroyable
	{
		[field: SerializeField] public GameObject Model { get; protected set; }
		[field: SerializeField] public DestroyableData Data { get; protected set; }

		public event Action<ISpawnable> OnSpawn;
		public event Action<IDestroyable> OnDestroy;
		
		private bool _isStopDestroy;
		
		public void StartDestroy()
		{
			StartCoroutine(Routine());
			return;

			IEnumerator Routine()
			{
				float timer = Data.TimeToDestroy;
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
		
		public virtual void Spawn()
		{
			OnSpawn?.Invoke(this);
			gameObject.SetActive(true);
		}

		public virtual void Destroy()
		{
			OnDestroy.Invoke(this);
			gameObject.SetActive(false);
		}
	}
}