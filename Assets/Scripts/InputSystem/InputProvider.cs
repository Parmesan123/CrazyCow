using System;
using System.Collections.Generic;
using System.Linq;

namespace InputSystem
{
	public class InputProvider
	{
		private readonly List<InputProfile> _inputProfiles;
		private readonly Type _defaultInput = typeof(JoyStickInput); 
		
		public InputProvider(List<InputProfile> inputProfiles)
		{
			_inputProfiles = inputProfiles;
		}

		public InputProfile GetProfile(Type inputType = null)
		{
			return inputType == null ?  
				_inputProfiles.First(i => i.GetType() == _defaultInput) :
				_inputProfiles.First(i => i.GetType() == inputType);
		}
	}
}