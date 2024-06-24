using Handlers;
using Saving;
using TMPro;
using UnityEngine;
using Wallet;
using Zenject;

namespace Installers
{
	public class HandlersInstaller : MonoInstaller
	{
		[SerializeField] private TextMeshProUGUI _text;
		
		public override void InstallBindings()
		{
			BindSaveHandler();
			BindWalletHandler();
			BindDestroyHandler();
			BindVaseHandler();
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
				.BindInterfacesAndSelfTo<GameWalletHandler>()
				.FromNew()
				.AsSingle();
		}

		private void BindDestroyHandler()
		{
			Container
				.BindInterfacesAndSelfTo<DestroyHandler>()
				.FromNew()
				.AsSingle();
		}
		
		private void BindVaseHandler()
		{
			Container
				.BindInterfacesAndSelfTo<VaseHandler>()
				.FromNew()
				.AsSingle();
		}
	}
}