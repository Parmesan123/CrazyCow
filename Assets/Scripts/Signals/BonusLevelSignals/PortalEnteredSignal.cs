using InteractableObject;
using Services;

namespace Signals
{
    public class PortalEnteredSignal : ISignal
    {
        public readonly Portal Portal;

        public PortalEnteredSignal(Portal portal)
        {
            Portal = portal;
        }
    }
}