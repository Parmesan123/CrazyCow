using System;
using UnityEngine;

public class PoolableBehaviour : MonoBehaviour, IPoolable
{
    public Action OnSpawn;
    public Action OnDestroy;

    private void OnEnable()
    {
        OnSpawn?.Invoke();
    }
    
    private void OnDisable()
    {
        OnDestroy?.Invoke();
    }
    
    public void Enable()
    {
        gameObject.SetActive(true);
    }
    
    public MonoBehaviour TryGetObject()
    {
        return gameObject.activeSelf ? null : this;
    }
    
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}