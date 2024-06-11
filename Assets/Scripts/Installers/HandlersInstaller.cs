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
			Container
				.Bind<WalletHandler>()
				.FromNew()
				.AsSingle()
				.WithArguments(_text);
		}

		private void BindDestroyHandler()
		{
			GameObject destroyHandler = new GameObject("DestroyHandler");

			Container.InstantiateComponent<DestroyHandler>(destroyHandler);
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