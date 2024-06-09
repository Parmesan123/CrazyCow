using UnityEngine;

namespace InteractableObject
{
	public class Portal : Interactable
	{
		protected override void Interact()
		{
			Debug.Log("Interact with portal");
		}
	}
}