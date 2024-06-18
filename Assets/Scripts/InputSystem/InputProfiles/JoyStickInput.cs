using System;
using System.Collections.Generic;
using Properties;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InputSystem
{
	public class JoyStickInput : InputProfile, IFixedUpdatable
	{
		private const string DATA_PATH = "Data/JoyStickData";

		public event Action<Vector2> OnMoveEvent;
		public event Action<bool, Vector2> OnTouchPerformedEvent;
		
		private readonly JoyStickData _joyStickData = Resources.Load<JoyStickData>(DATA_PATH);
		private Vector2? _touchPosition;
		private bool _isTouch;
		
		public void FixedUpdate()
		{
			CheckTouch();
			
			if(_touchPosition == null)
				return;
			
			OnMoveEvent.Invoke(GetMoveVector());
		}

		private void CheckTouch()
		{
			int touchCount = Input.touchCount;
			
			if (touchCount == 0)
			{
				if (_isTouch)
				{
					_isTouch = false;
					OnTouchPerformedEvent.Invoke(false, Vector2.zero);
				}
				
				_touchPosition = null;
				return;
			}

			if (_touchPosition != null)
				return;

			_touchPosition = Input.GetTouch(0).position;
			Vector2 resultTouch = (Vector2)_touchPosition;
			if (CheckTouchOnUI(resultTouch))
				return;
			
			_isTouch = true;
			OnTouchPerformedEvent.Invoke(true, resultTouch);
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

		private bool CheckTouchOnUI(Vector2 position)
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current)
			{
				position = position
			};

			List<RaycastResult> raycastResults = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, raycastResults);

			foreach (RaycastResult result in raycastResults)
				if (result.gameObject.TryGetComponent(out Button _))
					return true;

			return false;
		}
	}
}