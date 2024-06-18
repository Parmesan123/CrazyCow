using System.Linq;
using Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystemHandler : SaveableSingleton<SaveSystemHandler>
{
    [SerializeField] private GameData _gameData;

    private IDataService _serviceData;

    protected override void Awake()
    {
        base.Awake();
        
        _serviceData = new FileDataService(new JsonSerializer());
        
        LoadGame("save.json");
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0)
        {
            Bind<MenuWalletHandler, WalletSaveData>(_gameData.WalletData);
            return;
        }
            
        Bind<GameWalletHandler, WalletSaveData>(_gameData.WalletData);
    }

    private void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
    {
        T entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
        if (entity == null) 
            return;
        
        data ??= new TData() { Id = entity.Id };
        entity.Bind(data);
    }

    public void SaveGame()
    {
        _gameData ??= new GameData();

        _serviceData.Save(_gameData);
    }

    public void LoadGame(string saveName)
    {
        _gameData = _serviceData.Load(saveName);
    }
}