using System;

namespace Entities
{
    public interface IDestroyable
    {
        public event Action<IDestroyable> OnDestroyEvent;
    
        public void Destroy();
    }
}