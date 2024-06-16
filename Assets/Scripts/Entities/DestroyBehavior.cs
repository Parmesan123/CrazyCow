using System;
using UnityEngine;

namespace Entities
{
	[SelectionBase]
	public abstract class DestroyBehavior : MonoBehaviour, ISpawnable, IDestroyable
	{
		[field: SerializeField] public GameObject Model { get; protected set; }
		[field: SerializeField] public DestroyableData Data { get; protected set; }

		public Transform Transform => transform;
		public event Action<ISpawnable> OnSpawnEvent;
		public event Action<IDestroyable> OnDestroyEvent;
		
		
		public virtual void Spawn()
		{
			gameObject.SetActive(true);
			OnSpawnEvent?.Invoke(this);
		}

		public virtual void Destroy()
		{
			gameObject.SetActive(false);
			OnDestroyEvent?.Invoke(this);
		}
	}
}