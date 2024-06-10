using UnityEngine;
using Zenject;

namespace Services
{
    public abstract class MonoFactory<T>: IMonoFactory<T> where T: MonoBehaviour
    {
        protected DiContainer _container;
    
        protected MonoFactory(DiContainer container)
        {
            _container = container;
        }

        public abstract T CreateObject();
    }
}