using UnityEngine;

public interface IPoolable
{
    public MonoBehaviour TryGetObject();
}