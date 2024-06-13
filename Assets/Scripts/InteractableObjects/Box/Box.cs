using System;
using UnityEngine;

namespace InteractableObject
{
    public class Box : DestroyBehaviour, ICoinGiver
    {
        public event Action<ICoinGiver> OnCoinGive;

        public int AmountCoin => Data.AmountCoin;
        public Transform Transform => transform;

        public override void Destroy()
        {
            OnCoinGive.Invoke(this);
            
            base.Destroy();
        }
    }
}