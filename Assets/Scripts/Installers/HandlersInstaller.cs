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
				.AsSingle()
				.WithArguments(_text);
		}

		private void BindDestroyHandler()
		{
			Container
				.Bind<DestroyHandler>()
				.FromNew()
				.AsSingle();
		}
		
		private void BindVaseHandler()
		{
			Container
				.Bind<VaseHandler>()
				.FromNew()
				.AsSingle();
		}
	}
}