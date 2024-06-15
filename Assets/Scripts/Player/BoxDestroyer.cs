using InteractableObject;
using ModestTree;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Player
{
	public class BoxDestroyer : MonoBehaviour, IPausable
	{
		[SerializeField] private PlayerData _playerData;

		private readonly List<IDestroyable> _destroyables = new List<IDestroyable>();
		private DestroyBehaviour _currentDestroyBehaviour;
		
		private PauseHandler _pauseHandler;
		private BoxFactory _boxFactory;
		private VaseFactory _vaseFactory;
		
		[Inject]
		private void Construct(PauseHandler pauseHandler, BoxFactory boxFactory, VaseFactory vaseFactory)
		{
			_pauseHandler = pauseHandler;
			_pauseHandler.Register(this);

			_boxFactory = boxFactory;
			_boxFactory.OnDestroyBoxEvent += DestroyEntity;
			
			_vaseFactory = vaseFactory;
			_vaseFactory.OnDestroyVaseEvent += DestroyEntity;
		}
		
		private void Awake()
		{
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.isTrigger = true;
			sphereCollider.radius = _playerData.DestroyRange;
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
			
			_boxFactory.OnDestroyBoxEvent -= DestroyEntity;
			
			_vaseFactory.OnDestroyVaseEvent -= DestroyEntity;
		}
		
		public void Unpause()
		{
			//TODO: rework
			_boxFactory.OnDestroyBoxEvent += DestroyEntity;
			
			_vaseFactory.OnDestroyVaseEvent += DestroyEntity;
		}

		public void Pause()
		{
			//TODO: rework
			_boxFactory.OnDestroyBoxEvent -= DestroyEntity;
			
			_vaseFactory.OnDestroyVaseEvent -= DestroyEntity;
		}

		private void DestroyEntity(IDestroyable destroyable)
		{
			_destroyables.Remove(destroyable);
			
			SetNextCurrentDestroyable();
		}

		private void Register(DestroyBehaviour destroyBehaviour)
		{
			_destroyables.Add(destroyBehaviour);

			if (_currentDestroyBehaviour != null) 
				return;
			
			_currentDestroyBehaviour = destroyBehaviour;
			_currentDestroyBehaviour.StartDestroy();
		}

		private void UnRegister(DestroyBehaviour destroyBehaviour)
		{
			_destroyables.Remove(destroyBehaviour);

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