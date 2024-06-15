using System;
using UnityEngine;

namespace InteractableObject
{
    public class Box : DestroyBehaviour, ICoinGiver
    {
        public event Action<ICoinGiver> OnCoinGiveEvent;

        public int AmountCoin => Data.AmountCoin;
        public Transform Transform => transform;

        public override void Destroy()
        {
            OnCoinGiveEvent?.Invoke(this);
            
            base.Destroy();
        }
    }
}