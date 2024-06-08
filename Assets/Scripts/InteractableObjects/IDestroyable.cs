using System;

namespace InteractableObject
{
	public interface IDestroyable
	{
		public event Action<IDestroyable> OnDestroyEvent;
		
		public void StartDestroy();

		public void StopDestroy();
	}
}