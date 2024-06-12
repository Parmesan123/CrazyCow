using InteractableObject;
using Services;

public class VaseDestroyedByBotSignal : ISignal
{
    public readonly Vase Vase;

    public VaseDestroyedByBotSignal(Vase vase)
    {
        Vase = vase;
    }
}