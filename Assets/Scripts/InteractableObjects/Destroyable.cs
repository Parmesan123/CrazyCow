using System;
using System.Collections;
using UnityEngine;

namespace InteractableObject
{
	public abstract class Destroyable : MonoBehaviour
	{
		public event Action<Destroyable> OnDestroyEvent; 
        
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

		protected virtual void Destroy()
		{
			OnDestroyEvent.Invoke(this);
			gameObject.SetActive(false);
		}
	}
}