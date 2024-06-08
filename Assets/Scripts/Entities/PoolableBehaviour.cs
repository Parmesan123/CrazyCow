using UnityEngine;

public class PoolableBehaviour : MonoBehaviour, IPoolable
{
    public MonoBehaviour TryGetObject()
    {
        return gameObject.activeSelf ? null : this;
    }
}