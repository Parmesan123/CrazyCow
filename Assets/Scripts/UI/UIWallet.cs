using DG.Tweening;
using Handlers;
using TMPro;
using UnityEngine;
using Zenject;

public class UIWallet : MonoBehaviour
{
    [SerializeField] private GameObject _walletContainer;
    [SerializeField] private TextMeshProUGUI _text;

    private WalletHandler _walletHandler;
    private BonusLevelHandler _bonusLevelHandler;
    
    [Inject]
    private void Construct(WalletHandler walletHandler, BonusLevelHandler bonusLevelHandler)
    {
        _walletHandler = walletHandler;

        _bonusLevelHandler = bonusLevelHandler;
    }

    private void Awake()
    {
        UpdateWalletUI(0);
        
        _walletHandler.OnUIUpdateEvent += UpdateWalletUI;

        _bonusLevelHandler.OnBonusLevelStarted += DisableWalletUI;
        _bonusLevelHandler.OnBonusLevelEnded += EnableWalletUI;
    }

    private void OnDestroy()
    {
        _walletHandler.OnUIUpdateEvent -= UpdateWalletUI;
        
        _bonusLevelHandler.OnBonusLevelStarted -= DisableWalletUI;
        _bonusLevelHandler.OnBonusLevelEnded -= EnableWalletUI;
    }

    private void UpdateWalletUI(int coins)
    {
        _text.text = coins.ToString();
        _text.transform.localScale = Vector3.one;
        _text.transform.DOKill();
        _text.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f, 1);
    }

    private void EnableWalletUI()
    {
        _walletContainer.SetActive(true);
    }

    private void DisableWalletUI()
    {
        _walletContainer.SetActive(false);
    }
}