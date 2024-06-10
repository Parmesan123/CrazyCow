using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services
{
    public class Pool<T> where T: MonoBehaviour
    {
        private readonly List<T> _objects;
        private readonly IMonoFactory<T> _monoFactory;
        private readonly Transform _parent;
    
        public Pool(int initialSize, IMonoFactory<T> monoFactory, Transform parent)
        {
            _objects = new List<T>();
            _parent = parent;
        
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
            T firstObject = _objects.FirstOrDefault(o => o.gameObject.activeSelf == false);
            if (firstObject is not null)
                return firstObject;

            T newInstance = _monoFactory.CreateObject();
            newInstance.transform.parent = _parent;
            ObjectAdd(newInstance);
        
            return newInstance;
        }

        private void ObjectAdd(T newObject)
        {
            _objects.Add(newObject);
        }
    }
}