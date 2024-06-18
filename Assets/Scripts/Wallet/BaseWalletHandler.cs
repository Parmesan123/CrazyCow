using System;
using UnityEngine;

public class BaseWalletHandler : MonoBehaviour
{
    public event Action<int> OnUIUpdateEvent;

    protected int _coins;

    public virtual void UpdateCoins(int count)
    {
        _coins += count;
        
        OnUIUpdateEvent?.Invoke(_coins);
    }
}