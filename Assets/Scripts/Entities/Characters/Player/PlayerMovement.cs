using System;
using InputSystem;
using UnityEngine;
using Zenject;

namespace Entities
{
	public class PlayerMovement : MonoBehaviour
	{
		private Rigidbody _rigidbody;
		private JoyStickInput _input;
		
		public float CurrentSpeed { get; set; }
		
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
			Vector3 offSet = new Vector3(direction.x * Time.fixedDeltaTime * CurrentSpeed, 0,
				direction.y * Time.fixedDeltaTime * CurrentSpeed);
			Vector3 newPosition = transform.position + offSet;
			_rigidbody.MovePosition(newPosition);
		}
	}
}