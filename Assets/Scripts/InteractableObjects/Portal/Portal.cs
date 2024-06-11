using System.Collections;
using UnityEngine;

namespace InteractableObject
{
	public class Portal : Interactable, ISpawnable
	{
		private new PortalData _interactableData;

		private void Awake()
		{
			_interactableData = base._interactableData as PortalData;
		}

		public void Spawn()
		{
			StartCoroutine(TimerUntilDespawn());
		}
		
		protected override void Interact()
		{
			Debug.Log("Interact with portal");
		}

		private IEnumerator TimerUntilDespawn()
		{
			yield return new WaitForSeconds(_interactableData.TimeUntilDespawn);
			
			gameObject.SetActive(false);
		}
	}
}