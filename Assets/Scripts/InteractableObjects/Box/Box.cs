using System;
using UnityEngine;

namespace InteractableObject
{
    public class Box : DestroyBehaviour, ICoinGiver
    {
        public event Action<ICoinGiver> OnGiveCoinEvent;
        
        public Transform Transform { get; private set; }
        public int AmountCoin { get; private set; }

        private void Awake()
        {
            Transform = transform;
        }

        private void OnEnable()
        {
            AmountCoin = _data.AmountCoin;
        }

        public override void Destroy()
        {
            base.Destroy();
            OnGiveCoinEvent.Invoke(this);
        }
    }
}