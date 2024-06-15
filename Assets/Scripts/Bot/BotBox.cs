using InteractableObject;
using NaughtyAttributes;

namespace Bot
{
	public class BotBox : DestroyBehaviour
	{
		[Button]
		public void DestroyButton()
		{
			Destroy();
		}
	}
}