using InputSystem;
using InputSystem.InputProfiles;
using System;
using UnityEngine;
using Zenject;

namespace Player
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private PlayerData _playerData;
		
		private Rigidbody _rigidbody;
		private JoyStickInput _input;
		private float Speed => _playerData.PlayerSpeed;
		
		[Inject]
		private void Construct(InputProvider inputProvider)
		{
			_input = inputProvider.GetProfile(typeof(JoyStickInput)) as JoyStickInput;
		}

		private void Awake()
		{
			_rigidbody = GetComponentInChildren<Rigidbody>();

			if (_rigidbody == null)
				throw new NullReferenceException("Rigidbody component not found");
		}

		private void OnEnable()
		{
			_input.OnMoveEvent += Move;
		}

		private void OnDisable()
		{
			_input.OnMoveEvent -= Move;
		}

		private void Move(Vector2 direction)
		{
			Vector3 offSet = new Vector3(direction.x * Time.fixedDeltaTime * Speed, 0,
										 direction.y * Time.fixedDeltaTime * Speed);
			Vector3 newPosition = transform.position + offSet;
			_rigidbody.MovePosition(newPosition);
		}
	}
}