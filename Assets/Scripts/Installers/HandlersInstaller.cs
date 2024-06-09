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
		}

		private void BindWalletHandler()
		{
			Container
				.Bind<WalletHandler>()
				.FromNew()
				.AsSingle()
				.WithArguments(_text);
		}
	}
}