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
			};
			
			Container
				.Bind<InputProvider>()
				.FromNew()
				.AsSingle()
				.WithArguments(inputProfiles);
		}

		private void BindInputHandler()
		{
			GameObject inputHandler = new GameObject("InputHandler");
			
			Container.InstantiateComponent<InputHandler>(inputHandler);
		}
	}
}