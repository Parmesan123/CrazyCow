using InteractableObject;
using Services;

public class PortalEnteredSignal : ISignal
{
    public readonly Portal Portal;

    public PortalEnteredSignal(Portal portal)
    {
        Portal = portal;
    }
}