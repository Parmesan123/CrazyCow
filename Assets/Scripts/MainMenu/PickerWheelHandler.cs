using System;
using EasyUI.PickerWheelUI;
using Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wallet;
using Zenject;

namespace MainMenu
{
    public class PickerWheelHandler : MonoBehaviour, ICoinGiver
    {
        [SerializeField] private Button _uiSpinButton;
        [SerializeField] private TextMeshProUGUI _uiSpinButtonText;
        [SerializeField] private PickerWheel _pickerWheel;

        public event Action<ICoinGiver> OnCoinGiveEvent;
        public int AmountCoin { get; private set; }
        public Transform Transform => transform;

        private CoinSpawner _coinSpawner;

        [Inject]
        private void Construct(CoinSpawner coinSpawner)
        {
            _coinSpawner = coinSpawner;
        }

        private void OnEnable()
        {
            _uiSpinButtonText.text = "Spin";

            _uiSpinButton.onClick.AddListener(SpinWheel);
        }

        private void OnDisable()
        {
            _uiSpinButton.onClick.RemoveAllListeners();
        }

        private void SpinWheel()
        {
            _uiSpinButton.onClick.RemoveListener(SpinWheel);

            _uiSpinButton.interactable = false;
            _uiSpinButtonText.text = "Spinning";

            _pickerWheel.OnSpinEnd(CalculateReward);
            _pickerWheel.Spin();
        }

        private void CalculateReward(WheelPiece resultPiece)
        {
            _uiSpinButton.onClick.AddListener(ExitPickerWheel);
            _uiSpinButton.interactable = true;
            _uiSpinButtonText.text = "Exit";

            AmountCoin = resultPiece.Amount;
            _coinSpawner.Register(this);
            OnCoinGiveEvent.Invoke(this);

            _uiSpinButton.interactable = true;
            _uiSpinButtonText.text = "Spin";
        }

        private void ExitPickerWheel()
        {
            gameObject.SetActive(false);
        }
    }
}