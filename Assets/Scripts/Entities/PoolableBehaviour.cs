using UnityEngine;

public class PoolableBehaviour : MonoBehaviour, IPoolable
{
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