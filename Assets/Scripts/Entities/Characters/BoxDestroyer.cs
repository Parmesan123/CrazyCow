using System;
using ModestTree;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Entities
{
	public class BoxDestroyer : MonoBehaviour, IPausable
	{
		[field: SerializeField, Expandable] public CharacterData Data { get; private set; }

		public event Action<Vase> OnDestroyEvent;
		
		private List<IDestroyable> _destroyables;
		private DestroyBehavior _currentDestroyBehavior;
		private PauseHandler _pauseHandler;
		
		[Inject]
		private void Construct(PauseHandler pauseHandler)
		{
			_pauseHandler = pauseHandler;
			_pauseHandler.Register(this);
		}
		
		private void Awake()
		{
			_destroyables = new List<IDestroyable>();
			
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.isTrigger = true;
			sphereCollider.radius = Data.DestroyRange;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out DestroyBehavior destroyable))
				return;
			
			Register(destroyable);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent(out DestroyBehavior destroyable))
				return;
			
			UnRegister(destroyable);
		}

		private void OnDestroy()
		{
			_pauseHandler.Unregister(this);
		}

		private void OnDisable()
		{
			foreach (IDestroyable destroyable in _destroyables)
			{
				destroyable.OnDestroyEvent -= DestroyEntity;
				
				if (destroyable is DestroyBehavior convertable)
					convertable.StopDestroy();
			}
			
			_destroyables.Clear();
		}

		public void Unpause()
		{
		}

		public void Pause()
		{
		}

		private void DestroyEntity(IDestroyable destroyable)
		{
			UnRegister(destroyable as DestroyBehavior);
			
			if (destroyable is Vase vase)
				OnDestroyEvent?.Invoke(vase);
			
			SetNextCurrentDestroyable();
		}

		private void Register(DestroyBehavior destroyBehavior)
		{
			_destroyables.Add(destroyBehavior);
			destroyBehavior.OnDestroyEvent += DestroyEntity;

			if (_currentDestroyBehavior != null) 
				return;
			
			_currentDestroyBehavior = destroyBehavior;
			_currentDestroyBehavior.StartDestroy();
		}

		private void UnRegister(DestroyBehavior destroyBehavior)
		{
			_destroyables.Remove(destroyBehavior);
			destroyBehavior.OnDestroyEvent -= DestroyEntity;

			if (_currentDestroyBehavior != destroyBehavior) 
				return;
			
			_currentDestroyBehavior.StopDestroy();
			SetNextCurrentDestroyable();
		}

		private void SetNextCurrentDestroyable()
		{
			if (_destroyables.IsEmpty())
			{
				_currentDestroyBehavior = null;
				return;
			}
			
			_currentDestroyBehavior = _destroyables[0] as DestroyBehavior;
			_currentDestroyBehavior.StartDestroy();
		}
	}
}