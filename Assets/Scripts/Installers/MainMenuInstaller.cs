using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private UpgradeHandler _upgradeHandler;
    
    public override void InstallBindings()
    {
        BindSaveHandler();
        BindWalletHandler();
        BindMainMenuHandler();
        BindUpgradeHandler();
    }
    
    private void BindSaveHandler()
    {
        GameObject saveContainer = new GameObject("SaveHandler");

        SaveHandler saveHandler = Container.InstantiateComponent<SaveHandler>(saveContainer);
        saveHandler.Load();
        
        Container
            .Bind<SaveHandler>()
            .FromInstance(saveHandler)
            .AsSingle();
    }

    private void BindWalletHandler()
    {
        Container
            .Bind<MenuWalletHandler>()
            .FromNew()
            .AsSingle();
    }

    private void BindMainMenuHandler()
    {
        GameObject mainMenuContainer = new GameObject("MainMenuHandler");

        MainMenuHandler menuHandler = Container.InstantiateComponent<MainMenuHandler>(mainMenuContainer);
        
        Container
            .Bind<MainMenuHandler>()
            .FromInstance(menuHandler)
            .AsSingle();
    }

    private void BindUpgradeHandler()
    {
        Container
            .Bind<UpgradeHandler>()
            .FromInstance(_upgradeHandler)
            .AsSingle()
            .NonLazy();
    }
}