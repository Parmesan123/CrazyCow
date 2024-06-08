using InputSystem;
using InputSystem.InputProfiles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIJoyStick : MonoBehaviour
    {
        [SerializeField] private GameObject _joyStickUI;
        [SerializeField] private JoyStickData _joyStickData;

        private JoyStickInput _joyStickInput;

        [Inject]
        private void Construct(InputProvider inputProvider)
        {
            _joyStickInput = inputProvider.GetProfile(typeof(JoyStickInput)) as JoyStickInput;
        }
        
        private void Awake()
        {
            gameObject.SetActive(false);
            
            _joyStickInput.OnMoveEvent += MoveJoyStick;
            _joyStickInput.OnTouchPerformedEvent += SetActiveJoyStick;
        }
        
        private void OnDestroy()
        {
            _joyStickInput.OnMoveEvent -= MoveJoyStick;
            _joyStickInput.OnTouchPerformedEvent -= SetActiveJoyStick;
        }

        private void MoveJoyStick(Vector2 direction)
        {
            Vector3 offSet = new Vector3(direction.x * _joyStickData.JoyStickRadius, direction.y * _joyStickData.JoyStickRadius);
            
            _joyStickUI.transform.position = transform.position + offSet;
        }
        
        private void SetActiveJoyStick(bool value, Vector2 startPosition)
        {
            transform.position = startPosition;
            
            gameObject.SetActive(value);
        }
    }
}