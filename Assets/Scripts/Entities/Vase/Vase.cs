using System;

public class Vase : PoolableBehaviour
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
}