using InteractableObject;
using Services;

namespace Signals
{
    public class DestroyRemoveSignal : ISignal
    {
        public readonly DestroyBehaviour Destroyable;

        public DestroyRemoveSignal(DestroyBehaviour destroyable)
        {
            Destroyable = destroyable;
        }
    }
}