using System;
using UnityEngine;

namespace InteractableObject
{
	public class Portal : Interactable, ISpawnable
	{
		public event Action OnSpawn;
		
		protected override void Interact()
		{
			Debug.Log("Interact with portal");
		}

		public void Spawn()
		{
			throw new NotImplementedException();
		}
	}
}