using System;
using InputSystem;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Entities
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField, Expandable] private CharacterData _characterData;
		
		private Rigidbody _rigidbody;
		private JoyStickInput _input;
		
		private float Speed => _characterData.CharacterSpeed;
		
		[Inject]
		private void Construct(InputProvider inputProvider)
		{
			_input = inputProvider.GetProfile(typeof(JoyStickInput)) as JoyStickInput;
			_input.OnMoveEvent += MoveEvent;
		}

		private void Awake()
		{
			_rigidbody = GetComponentInChildren<Rigidbody>();

			if (_rigidbody == null)
				throw new NullReferenceException("Rigidbody component not found");
		}

		private void OnDestroy()
		{
			_input.OnMoveEvent -= MoveEvent;
		}

		private void MoveEvent(Vector2 direction)
		{
			Vector3 offSet = new Vector3(direction.x * Time.fixedDeltaTime * Speed, 0,
				direction.y * Time.fixedDeltaTime * Speed);
			Vector3 newPosition = transform.position + offSet;
			_rigidbody.MovePosition(newPosition);
		}
	}
}