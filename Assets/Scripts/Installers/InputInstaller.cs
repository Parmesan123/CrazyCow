using InputSystem;
using Zenject;
using UnityEngine;

namespace Installers
{
	public class InputInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindInputProvider();
			BindInputHandler();
		}

		private void BindInputProvider()
		{
			Container
				.Bind<InputProvider>()
				.FromNew()
				.AsSingle();
		}

		private void BindInputHandler()
		{
			GameObject inputHandler = new GameObject("InputHandler");
			
			Container.InstantiateComponent<InputHandler>(inputHandler);
		}
	}
}