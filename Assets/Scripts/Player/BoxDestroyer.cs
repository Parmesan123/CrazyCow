using System;
using InteractableObject;
using ModestTree;
using System.Collections.Generic;
using Services;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
	public class BoxDestroyer : MonoBehaviour, ISignalReceiver<DestroyRemoveSignal>
	{
		[SerializeField] private PlayerData _playerData;

		private readonly List<DestroyBehaviour> _destroyables = new List<DestroyBehaviour>();

		private DestroyBehaviour _currentDestroyBehaviour;

		private SignalBus _signalBus;
		
		[Inject]
		private void Construct(SignalBus signalBus)
		{
			_signalBus = signalBus;
		}
		
		private void Awake()
		{
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.isTrigger = true;
			sphereCollider.radius = _playerData.DestroyRange;
		}

		private void OnEnable()
		{
			_signalBus.Register<DestroyRemoveSignal>(this);
		}

		private void OnDisable()
		{
			_signalBus.Unregister<DestroyRemoveSignal>(this);
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
		
		public void Receive(DestroyRemoveSignal signal)
		{
			_destroyables.Remove(signal.Destroyable);
			
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
			
			_currentDestroyBehaviour = _destroyables[0];
			_currentDestroyBehaviour.StartDestroy();
		}
	}
}