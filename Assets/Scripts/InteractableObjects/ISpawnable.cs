using System;

public interface ISpawnable
{
    public event Action<ISpawnable> OnSpawn; 
    
    public void Spawn();
}