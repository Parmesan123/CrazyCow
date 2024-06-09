using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T: MonoBehaviour
{
    private readonly List<IPoolable> _objects;
    private readonly IMonoFactory<T> _monoFactory;
    private readonly Transform _parent;
    
    public Pool(int initialSize, IMonoFactory<T> monoFactory, Transform parent)
    {
        _parent = parent;
        _objects = new List<IPoolable>();
        
        _monoFactory = monoFactory;
        for (int i = 0; i < initialSize; i++)
        {
            T newInstance = _monoFactory.CreateObject();
            newInstance.transform.parent = parent;
            ObjectAdd(newInstance);
        }
    }
    
    public T ObjectGetFreeOrCreate()
    {
        foreach (IPoolable @object in _objects)
        {
            if (@object.TryGetObject() is T tObject)
                return tObject;
        }

        T newInstance = _monoFactory.CreateObject();
        newInstance.transform.parent = _parent;
        ObjectAdd(newInstance);
        
        return newInstance;
    }

    private void ObjectAdd(T newObject)
    {
        IPoolable poolable = newObject as IPoolable;
        _objects.Add(poolable);
    }
}