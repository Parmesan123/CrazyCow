using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Entities
{
	public abstract class Interactable : MonoBehaviour
	{
		[SerializeField, Expandable] protected InteractableData _interactableData;

		private Coroutine _timerCoroutine;
		
		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out PlayerMovement _))
				return;

			_timerCoroutine = StartCoroutine(StartTimer());
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent(out PlayerMovement _))
				return;
			
			StopTimer();
		}

		private IEnumerator StartTimer()
		{
			float timer = _interactableData.InteractTime;
			WaitForFixedUpdate wait = new WaitForFixedUpdate();
			
			for (; timer > 0;)
			{
				timer -= Time.fixedDeltaTime;

				yield return wait;
			}

			_timerCoroutine = null;
			Interact();
		}

		private void StopTimer()
		{
			if (_timerCoroutine == null)
				return;
			
			StopCoroutine(_timerCoroutine);
			_timerCoroutine = null;
		}

		protected abstract void Interact();
	}
}