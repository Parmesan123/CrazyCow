using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    private const int GAMEPLAY_SCENE_INDEX = 1;
    
    [Header("Buttons")]
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _storeButton;
    
    [Header("Containers")]
    [SerializeField] private GameObject _settingsContainer;
    [SerializeField] private GameObject _storeContainer;

    private void Awake()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _settingsButton.onClick.AddListener(OpenSettings);
        _storeButton.onClick.AddListener(OpenStore);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(GAMEPLAY_SCENE_INDEX);
    }

    private void OpenSettings()
    {
        _settingsContainer.SetActive(true);
    }

    private void OpenStore()
    {
        _storeContainer.SetActive(true);
    }
}