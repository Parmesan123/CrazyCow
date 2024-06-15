using System;

public interface ISpawnable
{
    public event Action<ISpawnable> OnSpawnEvent; 
    
    public void Spawn();
}