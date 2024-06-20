using System;
using UnityEngine;

public abstract class BaseWalletHandler : MonoBehaviour
{
    protected const string COINS_SAVE_KEY = "Coins";

    public event Action<int> OnUIUpdateEvent;

    protected int _coins;

    public virtual void UpdateCoins(int count)
    {
        _coins += count;
        
        OnUIUpdateEvent?.Invoke(_coins);
    }

    public abstract void Save();
}