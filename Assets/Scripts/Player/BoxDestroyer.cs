using InteractableObject;
using ModestTree;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Player
{
	public class BoxDestroyer : MonoBehaviour, IPausable
	{
		[field: SerializeField] public PlayerData Data { get; private set; }

		private readonly List<IDestroyable> _destroyables = new List<IDestroyable>();
		private DestroyBehaviour _currentDestroyBehaviour;
		
		private PauseHandler _pauseHandler;
		
		[Inject]
		private void Construct(PauseHandler pauseHandler)
		{
			_pauseHandler = pauseHandler;
			_pauseHandler.Register(this);
		}
		
		private void Awake()
		{
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.isTrigger = true;
			sphereCollider.radius = Data.DestroyRange;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out DestroyBehaviour destroyable))
				return;
			
			Register(destroyable);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent(out DestroyBehaviour destroyable))
				return;
			
			UnRegister(destroyable);
		}

		private void OnDestroy()
		{
			_pauseHandler.Unregister(this);
		}
		
		public void Unpause()
		{
		}

		public void Pause()
		{
		}

		private void DestroyEntity(IDestroyable destroyable)
		{
			_destroyables.Remove(destroyable);
			
			SetNextCurrentDestroyable();
		}

		private void Register(DestroyBehaviour destroyBehaviour)
		{
			_destroyables.Add(destroyBehaviour);
			destroyBehaviour.OnDestroyEvent += DestroyEntity;

			if (_currentDestroyBehaviour != null) 
				return;
			
			_currentDestroyBehaviour = destroyBehaviour;
			_currentDestroyBehaviour.StartDestroy();
		}

		private void UnRegister(DestroyBehaviour destroyBehaviour)
		{
			_destroyables.Remove(destroyBehaviour);
			destroyBehaviour.OnDestroyEvent -= DestroyEntity;

			if (_currentDestroyBehaviour != destroyBehaviour) 
				return;
			
			_currentDestroyBehaviour.StopDestroy();
			SetNextCurrentDestroyable();
		}

		private void SetNextCurrentDestroyable()
		{
			if (_destroyables.IsEmpty())
			{
				_currentDestroyBehaviour = null;
				return;
			}
			
			_currentDestroyBehaviour = _destroyables[0] as DestroyBehaviour;
			_currentDestroyBehaviour.StartDestroy();
		}
	}
}