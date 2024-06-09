using InteractableObject;
using ModestTree;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class BoxDestroyer : MonoBehaviour
	{
		[SerializeField] private PlayerData _playerData;

		private readonly List<DestroyBehaviour> _destroyables = new List<DestroyBehaviour>();

		private DestroyBehaviour _currentDestroyBehaviour;
		
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

		private void Register(DestroyBehaviour destroyBehaviour)
		{
			_destroyables.Add(destroyBehaviour);
			destroyBehaviour.OnDestroy += DestroyBoxListener;

			if (_currentDestroyBehaviour != null) 
				return;
			
			_currentDestroyBehaviour = destroyBehaviour;
			_currentDestroyBehaviour.StartDestroy();
		}

		private void UnRegister(DestroyBehaviour destroyBehaviour)
		{
			_destroyables.Remove(destroyBehaviour);
			destroyBehaviour.OnDestroy -= DestroyBoxListener;

			if (_currentDestroyBehaviour != destroyBehaviour) 
				return;
			
			_currentDestroyBehaviour.StopDestroy();
			SetNextCurrentDestroyable();
		}

		private void DestroyBoxListener(DestroyBehaviour destroyBehaviour)
		{
			_destroyables.Remove(destroyBehaviour);
			destroyBehaviour.OnDestroy -= DestroyBoxListener;
			
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