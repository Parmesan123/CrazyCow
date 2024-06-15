using System;

namespace Entities
{
    public interface ISpawnable
    {
        public event Action<ISpawnable> OnSpawnEvent; 
    
        public void Spawn();
    }
}