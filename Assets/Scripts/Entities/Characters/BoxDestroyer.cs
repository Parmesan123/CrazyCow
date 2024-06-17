using System;
using System.Collections;
using System.Collections.Generic;
using Handlers;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Entities
{
	public class BoxDestroyer : MonoBehaviour
	{
		[field: SerializeField, Expandable] public CharacterData Data { get; private set; }
		
		public event Action<Vase> OnDestroyEvent;

		private List<DestroyBehavior> _destroyables;
		private VaseHandler _vaseHandler;
		private (DestroyBehavior destroyBehavior, Coroutine coroutine) _destroyableCoroutine;
		private float _timer;
		
		[Inject]
		private void Construct(VaseHandler vaseHandler)
		{
			_destroyables = new List<DestroyBehavior>();

			_vaseHandler = vaseHandler;
		}
		
		private void Awake()
		{
			_vaseHandler.OnVaseDestroyedFromBoxEvent += DestroyVaseWithBox;
			
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.isTrigger = true;
			sphereCollider.radius = Data.DestroyRange;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out DestroyBehavior destroyable))
				return;
			
			_destroyables.Add(destroyable);
			destroyable.OnDestroyEvent += OnEntityDestroyed;
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent(out DestroyBehavior destroyable))
				return;
			
			_destroyables.Remove(destroyable);
			destroyable.OnDestroyEvent -= OnEntityDestroyed;
		}

		private void OnDisable()
		{
			_destroyables.Clear();
		}

		private void FixedUpdate()
		{
			ChangeDestroyable(GetClosestEntity());
		}
		
		private void ChangeDestroyable(DestroyBehavior destroyable)
		{
			if (destroyable == _destroyableCoroutine.destroyBehavior)
				return;
			
			if (_destroyableCoroutine.coroutine is not null)
				StopCoroutine(_destroyableCoroutine.coroutine);
			
			_destroyableCoroutine.destroyBehavior = destroyable;

			if (destroyable is null)
				_destroyableCoroutine.coroutine = null;
			
			if (_destroyableCoroutine.destroyBehavior is not null)
				StartDestroy(_destroyableCoroutine.destroyBehavior);
		}
		
		private void StartDestroy(DestroyBehavior destroyable)
		{
			_destroyableCoroutine.coroutine = StartCoroutine(Timer());
			return;

			IEnumerator Timer()
			{
				_timer = destroyable.Data.TimeToDestroy;
				
				for (; _timer >= 0;)
				{
					_timer -= Time.fixedDeltaTime;
                   
					yield return new WaitForFixedUpdate();
				}
                
				destroyable.Destroy();
			}
		}

		private DestroyBehavior GetClosestEntity()
		{
			DestroyBehavior closestDestroyable = null;
			
			float minDistance = float.PositiveInfinity;
			foreach (DestroyBehavior destroyable in _destroyables)
			{
				float distance = Vector3.Distance(destroyable.Transform.position, transform.position);
				if (distance < minDistance)
				{
					minDistance = distance;
					closestDestroyable = destroyable;
				}
			}
			
			return closestDestroyable;
		}
		
		private void OnEntityDestroyed(IDestroyable destroyable)
		{
			destroyable.OnDestroyEvent -= OnEntityDestroyed;
			_destroyables.Remove(destroyable as DestroyBehavior);
			StopCoroutine(_destroyableCoroutine.coroutine);
		}

		private void DestroyVaseWithBox(Box box, Vase vase)
		{
			if (_destroyableCoroutine.destroyBehavior != box)
				return;
			
			if (_timer > 0)
				return;
			
			OnDestroyEvent?.Invoke(vase);
		}
	}
}