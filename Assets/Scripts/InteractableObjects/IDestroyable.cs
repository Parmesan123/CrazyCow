using System;
using InteractableObject;

public interface IDestroyable
{
    public event Action<DestroyBehaviour> OnDestroy;

    public void Destroy();
}