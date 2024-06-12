using InteractableObject;
using Services;

public class VaseDestroyedByPlayerSignal : ISignal
{
    public readonly Vase Vase;

    public VaseDestroyedByPlayerSignal(Vase vase)
    {
        Vase = vase;
    }
}