using System;
using Properties;
using UnityEngine;

namespace InputSystem
{
	public class JoyStickInput : InputProfile, IFixedUpdatable
	{
		private const string DATA_PATH = "Data/JoyStickData";

		public event Action<Vector2> OnMove;
		public event Action<bool, Vector2> OnTouchPerformed;
		
		private readonly JoyStickData _joyStickData = Resources.Load<JoyStickData>(DATA_PATH);
		private Vector2? _touchPosition;
		private bool _isTouch;
		
		public void FixedUpdate()
		{
			CheckTouch();
			
			if(_touchPosition == null)
				return;
			
			OnMove.Invoke(GetMoveVector());
		}

		private void CheckTouch()
		{
			int touchCount = Input.touchCount;
			
			if (touchCount == 0)
			{
				if (_isTouch)
				{
					_isTouch = false;
					OnTouchPerformed.Invoke(false, Vector2.zero);
				}
				
				_touchPosition = null;
				return;
			}

			if (_touchPosition != null)
				return;
			
			_touchPosition = Input.GetTouch(0).position;
			_isTouch = true;
			OnTouchPerformed.Invoke(true, (Vector2)_touchPosition);
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