using System;

public class Crate : PoolableBehaviour
{
    public Action OnDeath;

    private void OnDisable()
    {
        OnDeath?.Invoke();
    }
}