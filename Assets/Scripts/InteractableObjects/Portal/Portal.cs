using System.Collections;
using Services;
using Signals;
using UnityEngine;
using Zenject;

namespace InteractableObject
{
	public class Portal : Interactable, ISpawnable
	{
		private new PortalData _interactableData;

		private SignalBus _signalBus;
		
		[Inject]
		private void Construct(SignalBus signalBus)
		{
			_signalBus = signalBus;
		}
		
		private void Awake()
		{
			_interactableData = base._interactableData as PortalData;
		}

		public void Spawn()
		{
			gameObject.SetActive(true);
			StartCoroutine(TimerUntilDespawn());
		}
		
		protected override void Interact()
		{
			_signalBus.Invoke(new PortalEnteredSignal(this));
		}

		private IEnumerator TimerUntilDespawn()
		{
			yield return new WaitForSeconds(_interactableData.TimeUntilDespawn);
			
			gameObject.SetActive(false);
		}
	}
}