using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class GameWalletUI : MonoBehaviour
{
    [SerializeField] private GameObject _walletContainer;
    [SerializeField] private TextMeshProUGUI _text;

    private IWallet _walletHandler;
    private BonusLevelHandler _bonusLevelHandler;
    
    [Inject]
    private void Construct(IWallet gameWalletHandler, BonusLevelHandler bonusLevelHandler)
    {
        _walletHandler = gameWalletHandler;

        _bonusLevelHandler = bonusLevelHandler;
    }

    private void Awake()
    {
        UpdateGameWalletUI(0);
        
        _walletHandler.OnCashUpdatedEvent += UpdateGameWalletUI;

        _bonusLevelHandler.OnBonusLevelStarted += DisableWalletUI;
        _bonusLevelHandler.OnBonusLevelEnded += EnableWalletUI;
    }

    private void OnDestroy()
    {
        _walletHandler.OnCashUpdatedEvent -= UpdateGameWalletUI;
        
        _bonusLevelHandler.OnBonusLevelStarted -= DisableWalletUI;
        _bonusLevelHandler.OnBonusLevelEnded -= EnableWalletUI;
    }

    private void UpdateGameWalletUI(int coins)
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