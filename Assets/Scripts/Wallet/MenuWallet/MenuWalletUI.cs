using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Wallet
{
    public class MenuWalletUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private MenuWalletHandler _menuWalletHandler;
    
        [Inject]
        private void Construct(MenuWalletHandler walletHandler)
        {
            _menuWalletHandler = walletHandler;
        }
    
        private void Awake()
        {
            _menuWalletHandler.OnCashUpdatedEvent += CashUpdatedEvent;
            _menuWalletHandler.OnCashSpendFailedEvent += CashSpendFailedEvent;
        }

        private void OnDestroy()
        {
            _menuWalletHandler.OnCashUpdatedEvent -= CashUpdatedEvent;
            _menuWalletHandler.OnCashSpendFailedEvent -= CashSpendFailedEvent;
        }

        private void CashUpdatedEvent(int coins)
        {
            _text.text = coins.ToString();
        
            _text.color = Color.green;
            _text.transform.localScale = Vector3.one;
            _text.transform.DOKill();
            _text.transform
                .DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1)
                .OnComplete(() => _text.color = Color.white);
        }

        private void CashSpendFailedEvent()
        {
            _text.color = Color.red;
            _text.transform.localScale = Vector3.one;
            _text.transform.DOKill();
            _text.transform
                .DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1)
                .OnComplete(() => _text.color = Color.white);
        }
    }
}