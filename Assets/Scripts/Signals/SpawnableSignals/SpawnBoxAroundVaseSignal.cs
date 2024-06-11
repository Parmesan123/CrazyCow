using InteractableObject;
using Services;

public class SpawnBoxAroundVaseSignal : ISignal
{
    public readonly Box Box;

    public SpawnBoxAroundVaseSignal(Box box)
    {
        Box = box;
    }
}