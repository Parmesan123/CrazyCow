using UnityEngine;
using Zenject;

public abstract class MonoFactory<T>: IMonoFactory<T> where T: MonoBehaviour
{
    protected DiContainer _container;
    
    protected MonoFactory(DiContainer container)
    {
        _container = container;
    }

    public abstract T CreateObject();
}