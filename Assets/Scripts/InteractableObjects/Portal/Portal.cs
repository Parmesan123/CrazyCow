using System;
using System.Collections;
using UnityEngine;

namespace InteractableObject
{
	public class Portal : Interactable, ISpawnable
	{
		private new PortalData _interactableData;
		
		public event Action<ISpawnable> OnSpawnEvent;
		public event Action OnEnter;
		
		private void Awake()
		{
			_interactableData = base._interactableData as PortalData;
		}
		
		public void Spawn()
		{
			OnSpawnEvent?.Invoke(this);
			
			gameObject.SetActive(true);
			StartCoroutine(TimerUntilDespawn());
		}
		
		protected override void Interact()
		{
			OnEnter.Invoke();
		}

		private IEnumerator TimerUntilDespawn()
		{
			yield return new WaitForSeconds(_interactableData.TimeUntilDespawn);
			
			gameObject.SetActive(false);
		}
	}
}