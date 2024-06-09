using System;

namespace InputSystem
{
	public abstract class InputProfile
	{
		public event Action<Type> OnChangeInputEvent;

		protected void InvokeEvent(Type newInputType)
		{
			OnChangeInputEvent.Invoke(newInputType);
		}
	}
}