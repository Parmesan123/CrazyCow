using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MainMenuHandler : MonoBehaviour
{
    private const int GAMEPLAY_SCENE_INDEX = 1;

    private MenuWalletHandler _wallet;
    private SaveHandler _saveHandler;

    [Inject]
    private void Construct(MenuWalletHandler walletHandler, SaveHandler saveHandler)
    {
        _wallet = walletHandler;

        _saveHandler = saveHandler;
    }

    private void Start()
    {
        UpdateWallet();
    }

    private void UpdateWallet()
    {
        _wallet.UpdateCoins();
    }

    public void StartGame()
    {
        _saveHandler.Save();
        
        SceneManager.LoadScene(GAMEPLAY_SCENE_INDEX);
    }
}