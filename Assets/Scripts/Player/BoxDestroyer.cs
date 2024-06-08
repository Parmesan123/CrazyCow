using InteractableObject;
using ModestTree;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class BoxDestroyer : MonoBehaviour
	{
		[SerializeField] private PlayerData _playerData;

		private readonly List<IDestroyable> _destroyables = new List<IDestroyable>();

		private IDestroyable _currentDestroyable;
		
		private void Awake()
		{
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.isTrigger = true;
			sphereCollider.radius = _playerData.DestroyRange;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out IDestroyable destroyable))
				return;
			
			Register(destroyable);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent(out IDestroyable destroyable))
				return;
			
			UnRegister(destroyable);
		}

		private void Register(IDestroyable destroyable)
		{
			_destroyables.Add(destroyable);
			destroyable.OnDestroyEvent += DestroyBoxListener;

			if (_currentDestroyable != null) 
				return;
			
			_currentDestroyable = destroyable;
			_currentDestroyable.StartDestroy();
		}

		private void UnRegister(IDestroyable destroyable)
		{
			_destroyables.Remove(destroyable);
			destroyable.OnDestroyEvent -= DestroyBoxListener;

			if (_currentDestroyable != destroyable) 
				return;
			
			_currentDestroyable.StopDestroy();
			SetNextCurrentDestroyable();
		}

		private void DestroyBoxListener(IDestroyable destroyable)
		{
			_destroyables.Remove(destroyable);
			destroyable.OnDestroyEvent -= DestroyBoxListener;
			
			SetNextCurrentDestroyable();
		}

		private void SetNextCurrentDestroyable()
		{
			if (_destroyables.IsEmpty())
			{
				_currentDestroyable = null;
				return;
			}
			
			_currentDestroyable = _destroyables[0];
			_currentDestroyable.StartDestroy();
		}
	}
}