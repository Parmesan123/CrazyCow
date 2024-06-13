using InputSystem;
using Services;
using Signals;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIJoyStick : MonoBehaviour, ISignalReceiver<MoveSignal>, ISignalReceiver<TouchPerformedSignal>
    {
        [SerializeField] private GameObject _joyStickGameObject;
        [SerializeField] private GameObject _joyStickUI;
        [SerializeField] private JoyStickData _joyStickData;

        private SignalBus _signalBus;
        private JoyStickInput _joyStickInput;

        [Inject]
        private void Construct(SignalBus signalBus, InputProvider inputProvider)
        {
            _signalBus = signalBus;
            _joyStickInput = inputProvider.GetProfile(typeof(JoyStickInput)) as JoyStickInput;
        }
        
        private void Awake()
        {
            _joyStickGameObject.SetActive(false);
            
            _signalBus.RegisterUnique<MoveSignal>(this);
            _signalBus.RegisterUnique<TouchPerformedSignal>(this);
        }
        
        private void OnDestroy()
        {
            if (_joyStickInput == null)
                return;
            
            _signalBus.Unregister<MoveSignal>(this);
            _signalBus.Unregister<TouchPerformedSignal>(this);
        }
        
        public void Receive(MoveSignal signal)
        {
            Vector2 direction = signal.MovementVector;
            
            Vector3 offSet = new Vector3(direction.x * _joyStickData.JoyStickRadius, 
                direction.y * _joyStickData.JoyStickRadius);
            
            _joyStickUI.transform.position = transform.position + offSet;
        }

        public void Receive(TouchPerformedSignal signal)
        {
            transform.position = signal.TouchPosition;
            
            _joyStickGameObject.SetActive(signal.IsTouched);
        }
    }
}