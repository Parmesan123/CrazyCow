using InteractableObject;
using Services;

public class SpawnVaseAroundCrateSignal : ISignal
{
    public readonly Vase Vase;

    public SpawnVaseAroundCrateSignal(Vase vase)
    {
        Vase = vase;
    }
}