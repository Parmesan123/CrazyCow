﻿using Services;
using UnityEngine;

namespace Signals
{
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
}