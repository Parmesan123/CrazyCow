using Services;

public class DestroyEntitySignal : ISignal
{
    public readonly ISpawnable Spawnable;

    public DestroyEntitySignal(ISpawnable spawnable)
    {
        Spawnable = spawnable;
    }
}