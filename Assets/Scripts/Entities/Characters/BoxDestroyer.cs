using System;
using System.Collections;
using System.Collections.Generic;
using Handlers;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Entities
{
	public class BoxDestroyer : MonoBehaviour, IPausable
	{
		[field: SerializeField, Expandable] public CharacterData Data { get; private set; }
		
		public event Action<Vase> OnDestroyEvent;

		private List<DestroyBehavior> _destroyables;
		private VaseHandler _vaseHandler;
		private (DestroyBehavior destroyBehavior, Coroutine coroutine) _destroyableCoroutine;
		private PauseHandler _pauseHandler;
		
		[Inject]
		private void Construct(PauseHandler pauseHandler, VaseHandler vaseHandler)
		{
			_pauseHandler = pauseHandler;
			_pauseHandler.Register(this);
			
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

		private void OnDestroy()
		{
			_pauseHandler.Unregister(this);
		}

		private void OnDisable()
		{
			_destroyables.Clear();
		}

		public void Unpause()
		{
		}

		public void Pause()
		{
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
				float timer = destroyable.Data.TimeToDestroy;
				
				for (; timer >= 0;)
				{
					timer -= Time.fixedDeltaTime;
                   
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
			
			OnDestroyEvent?.Invoke(vase);
		}
	}
}