using DG.Tweening;
using Handlers;
using TMPro;
using UnityEngine;
using Zenject;

public class GameWalletUI : MonoBehaviour
{
    [SerializeField] private GameObject _walletContainer;
    [SerializeField] private TextMeshProUGUI _text;

    private GameWalletHandler _gameWalletHandler;
    private BonusLevelHandler _bonusLevelHandler;
    
    [Inject]
    private void Construct(GameWalletHandler gameWalletHandler, BonusLevelHandler bonusLevelHandler)
    {
        _gameWalletHandler = gameWalletHandler;

        _bonusLevelHandler = bonusLevelHandler;
    }

    private void Awake()
    {
        UpdateGameWalletUI(0);
        
        _gameWalletHandler.OnUIUpdateEvent += UpdateGameWalletUI;

        _bonusLevelHandler.OnBonusLevelStarted += DisableWalletUI;
        _bonusLevelHandler.OnBonusLevelEnded += EnableWalletUI;
    }

    private void OnDestroy()
    {
        _gameWalletHandler.OnUIUpdateEvent -= UpdateGameWalletUI;
        
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