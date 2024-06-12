using Services;
using UnityEngine;

public class MoveSignal : ISignal
{
    public readonly Vector2 MovementVector;

    public MoveSignal(Vector2 movementVector)
    {
        MovementVector = movementVector;
    }
}