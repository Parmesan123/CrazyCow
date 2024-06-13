using System.Collections;
using Services;
using Signals;
using UnityEngine;
using Zenject;

namespace InteractableObject
{
	[SelectionBase]
	public abstract class DestroyBehaviour : MonoBehaviour, ISpawnable, IDestroyable
	{
		[SerializeField] private GameObject _model;
		[SerializeField] protected DestroyableData _data;
		
		protected SignalBus SignalBus;
		
		private bool _isStopDestroy;

		[Inject]
		public void Construct(SignalBus signalBus)
		{
			SignalBus = signalBus;
		}
		
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
			SignalBus.Invoke(new DestroyEntitySignal(this));
			SignalBus.Invoke(new DestroyAnimationSignal(_data, _model));
			gameObject.SetActive(false);
		}

		public virtual void Spawn()
		{
			gameObject.SetActive(true);
		}
	}
}