using Properties;
using System;
using UnityEngine;
using Zenject;

namespace InputSystem
{
	public class InputHandler : MonoBehaviour
	{
		private InputProfile _currentProfile;
		private IUpdatable _updatable;
		private IFixedUpdatable _fixedUpdatable;
		private InputProvider _inputProvider;

		[Inject]
		private void Construct(InputProvider inputProvider)
		{
			_inputProvider = inputProvider;
			ChangeInputProfile();
		}
		
		private void Update()
		{
			_updatable?.Update();
		}

		private void FixedUpdate()
		{
			_fixedUpdatable?.FixedUpdate();
		}

		private void ChangeInputProfile(Type inputType = null)
		{
			InputProfile newProfile = _inputProvider.GetProfile(inputType);

			if (_currentProfile != null)
				_currentProfile.OnChangeInputEvent -= ChangeInputProfile;

			_currentProfile = newProfile;
			_currentProfile.OnChangeInputEvent += ChangeInputProfile;
			_fixedUpdatable = _currentProfile as IFixedUpdatable;
			_updatable = _currentProfile as IUpdatable;
		}
	}
}