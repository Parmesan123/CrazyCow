using System;

namespace Entities
{
    public class Box : DestroyBehavior, ICoinGiver
    {
        public event Action<ICoinGiver> OnCoinGiveEvent;

        public int AmountCoin => Data.AmountCoin;

        public override void Destroy()
        {
            OnCoinGiveEvent?.Invoke(this);
            
            base.Destroy();
        }
    }
}