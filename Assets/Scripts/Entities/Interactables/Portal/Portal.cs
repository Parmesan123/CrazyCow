using System;
using System.Collections;
using UnityEngine;

namespace Entities
{
	public class Portal : InteractableBehaviour, ISpawnable
	{
		private PortalData _portalData;
		
		public event Action<ISpawnable> OnSpawnEvent;
		public event Action OnEnter;
		
		private void Awake()
		{
			_portalData = _interactableData as PortalData;
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
			gameObject.SetActive(false);
		}

		private IEnumerator TimerUntilDespawn()
		{
			yield return new WaitForSeconds(_portalData.TimeUntilDespawn);
			
			gameObject.SetActive(false);
		}
	}
}