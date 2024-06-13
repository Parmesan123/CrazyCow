using System;
using InputSystem;
using UnityEngine;
using Zenject;

namespace Player
{
	public class PlayerMovement : MonoBehaviour, IPausable
	{
		[SerializeField] private PlayerData _playerData;
		
		private Rigidbody _rigidbody;
		private JoyStickInput _input;
		private PauseHandler _pauseHandler;
		
		private float Speed => _playerData.PlayerSpeed;
		
		[Inject]
		private void Construct(PauseHandler pauseHandler, InputProvider inputProvider)
		{
			_pauseHandler = pauseHandler;
			_pauseHandler.Register(this);
			
			_input = inputProvider.GetProfile(typeof(JoyStickInput)) as JoyStickInput;
			_input.OnMove += Move;
		}

		private void Awake()
		{
			_rigidbody = GetComponentInChildren<Rigidbody>();

			if (_rigidbody == null)
				throw new NullReferenceException("Rigidbody component not found");
		}

		private void OnDestroy()
		{
			_pauseHandler.Unregister(this);

			_input.OnMove -= Move;
		}
		
		public void Pause()
		{
			//TODO: rework
			_input.OnMove -= Move;
		}

		public void Unpause()
		{
			//TODO: rework
			_input.OnMove += Move;
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