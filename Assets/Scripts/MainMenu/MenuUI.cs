using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _storeButton;
    
    [Header("Containers")]
    [SerializeField] private GameObject _settingsContainer;
    [SerializeField] private GameObject _storeContainer;

    private MainMenuHandler _menuHandler;
    
    [Inject]
    private void Construct(MainMenuHandler mainMenuHandler)
    {
        _menuHandler = mainMenuHandler;
    }

    private void Awake()
    {
        _startGameButton.onClick.AddListener(OnStartGamePressed);
        _settingsButton.onClick.AddListener(OnSettingsPressed);
        _storeButton.onClick.AddListener(OnStorePressed);
    }

    private void OnStartGamePressed()
    {
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