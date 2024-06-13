using Properties;
using Services;
using Signals;
using UnityEngine;

namespace InputSystem
{
	public class JoyStickInput : InputProfile, IFixedUpdatable
	{
		private const string DATA_PATH = "Data/JoyStickData";

		private readonly SignalBus _signalBus;
		
		private readonly JoyStickData _joyStickData = Resources.Load<JoyStickData>(DATA_PATH);
		private Vector2? _touchPosition;
		private bool _isTouch;

		public JoyStickInput(SignalBus signalBus)
		{
			_signalBus = signalBus;
		}
		
		public void FixedUpdate()
		{
			CheckTouch();
			
			if(_touchPosition == null)
				return;
			
			_signalBus.Invoke(new MoveSignal(GetMoveVector()));
		}

		private void CheckTouch()
		{
			int touchCount = Input.touchCount;
			
			if (touchCount == 0)
			{
				if (_isTouch)
				{
					_isTouch = false;
					_signalBus.Invoke(new TouchPerformedSignal(false, Vector2.zero));
				}
				
				_touchPosition = null;
				return;
			}

			if (_touchPosition != null)
				return;
			
			_touchPosition = Input.GetTouch(0).position;
			_isTouch = true;
			_signalBus.Invoke(new TouchPerformedSignal(true, (Vector2)_touchPosition));
		}

		private Vector2 GetMoveVector()
		{
			Vector2 offSet = Input.GetTouch(0).position;
			
			Vector2 moveVector = offSet - (Vector2)_touchPosition;

			if (moveVector.magnitude < _joyStickData.DeadZone)
				return Vector2.zero;

			if (moveVector.magnitude > _joyStickData.JoyStickRadius)
				return moveVector.normalized;
			
			return moveVector / _joyStickData.JoyStickRadius;
		}
	}
}