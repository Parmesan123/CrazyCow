using System;
using Handlers;
using InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class PauseUI : MonoBehaviour
{
    private const int MAIN_MENU_SCENE_INDEX = 0;
    
    [Header("Buttons")]
    [SerializeField] private Button _exitGameButton;
    [SerializeField] private Button _openPauseMenuButton;
    [SerializeField] private Button _closePauseMenuButton;

    [Header("Containers")]
    [SerializeField] private GameObject _pauseMenuContainer;

    private GameWalletHandler _walletHandler;
    private InputHandler _inputHandler;

    [Inject]
    private void Construct(GameWalletHandler walletHandler, InputHandler inputHandler)
    {
        _walletHandler = walletHandler;

        _inputHandler = inputHandler;
    }
    
    private void Awake()
    {
        _exitGameButton.onClick.AddListener(ExitGame);
        _openPauseMenuButton.onClick.AddListener(OpenPauseMenu);
        _closePauseMenuButton.onClick.AddListener(ClosePauseMenu);
    }

    private void ExitGame()
    {
        _walletHandler.SaveData();
        
        SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
    }

    private void OpenPauseMenu()
    {
        _pauseMenuContainer.SetActive(true);
        
        _inputHandler.ChangeInputProfile(typeof(EmptyInput));
    }

    private void ClosePauseMenu()
    {
        _pauseMenuContainer.SetActive(false);
        
        _inputHandler.ChangeInputProfile(typeof(JoyStickInput));
    }
}