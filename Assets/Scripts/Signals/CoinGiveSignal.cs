using Services;
using UnityEngine;

public class CoinGiveSignal : ISignal
{
    public readonly Transform Transform;
    public readonly int AmountCoin;

    public CoinGiveSignal(Transform transform, int amountCoin)
    {
        Transform = transform;
        AmountCoin = amountCoin;
    }
}