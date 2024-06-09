using System;

public interface ISpawnable
{
    public event Action OnSpawn;
    
    public void Spawn();
}