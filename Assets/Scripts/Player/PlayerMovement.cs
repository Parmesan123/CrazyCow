using InputSystem;
using InputSystem.InputProfiles;
using System;
using Services;
using UnityEngine;
using Zenject;

namespace Player
{
	public class PlayerMovement : MonoBehaviour, IPausable, ISignalReceiver<MoveSignal>
	{
		[SerializeField] private PlayerData _playerData;
		
		private Rigidbody _rigidbody;
		private JoyStickInput _input;
		private PauseHandler _pauseHandler;
		private SignalBus _signalBus;
		
		private float Speed => _playerData.PlayerSpeed;
		
		[Inject]
		private void Construct(SignalBus signalBus, InputProvider inputProvider, PauseHandler pauseHandler)
		{
			_signalBus = signalBus;
			
			_input = inputProvider.GetProfile(typeof(JoyStickInput)) as JoyStickInput;
			_pauseHandler = pauseHandler;
			
			_pauseHandler.Register(this);
			_signalBus.RegisterUnique<MoveSignal>(this);
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
		}

		public void Pause()
		{
			_signalBus.Unregister<MoveSignal>(this);
		}

		public void Unpause()
		{
			_signalBus.RegisterUnique<MoveSignal>(this);
		}

		public void Receive(MoveSignal signal)
		{
			Vector2 direction = signal.MovementVector;
			
			Vector3 offSet = new Vector3(direction.x * Time.fixedDeltaTime * Speed, 0,
				direction.y * Time.fixedDeltaTime * Speed);
			Vector3 newPosition = transform.position + offSet;
			_rigidbody.MovePosition(newPosition);
		}
	}
}