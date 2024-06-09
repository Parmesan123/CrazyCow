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
			BindCoinHandler();
		}

		private void BindCoinHandler()
		{
			Container
				.Bind<CoinHandler>()
				.FromNew()
				.AsSingle()
				.WithArguments(_text);
		}
	}
}