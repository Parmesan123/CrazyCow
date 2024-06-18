using Handlers;
using TMPro;
using UnityEngine;
using Zenject;

namespace Installers
{
	public class HandlersInstaller : MonoInstaller
	{
		[SerializeField] private TextMeshProUGUI _text;
		
		public override void InstallBindings()
		{
			BindWalletHandler();
			BindDestroyHandler();
			BindVaseHandler();
		}

		private void BindWalletHandler()
		{
			GameObject walletContainer = new GameObject("WalletHandler");
			GameWalletHandler walletHandler = Container.InstantiateComponent<GameWalletHandler>(walletContainer);
			
			Container
				.Bind<GameWalletHandler>()
				.FromInstance(walletHandler)
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