using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuUI : MonoBehaviour
{
    private const int START_GAME_COST = 100;
    
    [Header("Buttons")]
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _storeButton;
    
    [Header("Containers")]
    [SerializeField] private GameObject _settingsContainer;
    [SerializeField] private GameObject _storeContainer;
    [SerializeField] private GameObject _pickerWheelContainer;

    private MainMenuHandler _menuHandler;
    private MenuWalletHandler _walletHandler;
    
    [Inject]
    private void Construct(MainMenuHandler mainMenuHandler, MenuWalletHandler walletHandler)
    {
        _menuHandler = mainMenuHandler;

        _walletHandler = walletHandler;
    }

    private void Awake()
    {
        _startGameButton.onClick.AddListener(OnStartGamePressed);
        _settingsButton.onClick.AddListener(OnSettingsPressed);
        _storeButton.onClick.AddListener(OnStorePressed);
    }

    private void OnStartGamePressed()
    {
        if (!_walletHandler.TryRemoveCoins(START_GAME_COST))
        {
            _pickerWheelContainer.SetActive(true);
            return;
        }
        
        _menuHandler.StartGame();
    }

    private void OnSettingsPressed()
    {
        _settingsContainer.SetActive(true);
    }

    private void OnStorePressed()
    {
        _storeContainer.SetActive(true);
    }
}