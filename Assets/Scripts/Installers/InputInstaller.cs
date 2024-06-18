using System.Collections.Generic;
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
			List<InputProfile> inputProfiles = new List<InputProfile>
			{
				new JoyStickInput(),
				new EmptyInput(),
			};
			
			Container
				.Bind<InputProvider>()
				.FromNew()
				.AsSingle()
				.WithArguments(inputProfiles);
		}

		private void BindInputHandler()
		{
			GameObject inputContainer = new GameObject("InputHandler");
			
			InputHandler inputHandler = Container.InstantiateComponent<InputHandler>(inputContainer);

			Container
				.Bind<InputHandler>()
				.FromInstance(inputHandler)
				.AsSingle();
		}
	}
}