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
			BindPauseHandler();
			BindWalletHandler();
			BindDestroyHandler();
			BindVaseHandler();
		}

		private void BindPauseHandler()
		{
			Container
				.Bind<PauseHandler>()
				.FromNew()
				.AsSingle();
		}

		private void BindWalletHandler()
		{
			Container
				.Bind<WalletHandler>()
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