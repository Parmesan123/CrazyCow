using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace InteractableObject
{
	[SelectionBase]
	public abstract class DestroyBehaviour : MonoBehaviour, ISpawnable, IDestroyable
	{
		[field: SerializeField] public GameObject Model { get; protected set; }
		[field: SerializeField] public DestroyableData Data { get; protected set; }

		public event Action<ISpawnable> OnSpawnEvent;
		public event Action<IDestroyable> OnDestroyEvent;
		
		private bool _isStopDestroy;
		
		public void StartDestroy()
		{
			Timer();
			return;

			async void Timer()
			{
				float timer = Data.TimeToDestroy;
				
				_isStopDestroy = false;
                
				for (; timer >= 0;)
				{
					timer -= Time.fixedDeltaTime;
                   
					await Task.Delay(TimeSpan.FromSeconds(Time.fixedDeltaTime));
                    
					if (_isStopDestroy)
						return;
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
			gameObject.SetActive(true);
			OnSpawnEvent?.Invoke(this);
		}

		public virtual void Destroy()
		{
			gameObject.SetActive(false);
			OnDestroyEvent.Invoke(this);
		}
	}
}