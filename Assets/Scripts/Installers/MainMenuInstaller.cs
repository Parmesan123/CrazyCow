using MainMenu;
using Saving;
using Skills;
using Store;
using UnityEngine;
using Wallet;
using Zenject;

namespace Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private UpgradeProvider _upgradeProvider;
        [SerializeField] private CoinSpawner _coinSpawner;
        
        public override void InstallBindings()
        {
            BindSaveHandler();
            BindWalletHandler();
            BindMainMenuHandler();
            BindUpgradeHandler();
            BindCoinSpawner();
            BindSkillProvider();
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
                .BindInterfacesAndSelfTo<MenuWalletHandler>()
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
                .Bind<UpgradeProvider>()
                .FromInstance(_upgradeProvider)
                .AsSingle()
                .NonLazy();
        }
        
        private void BindCoinSpawner()
        {
            Container
                .Bind<CoinSpawner>()
                .FromInstance(_coinSpawner)
                .AsSingle();
        }
        
        private void BindSkillProvider()
        {
            Container
                .Bind<SkillProvider>()
                .FromNew()
                .AsSingle();
        }
    }
}