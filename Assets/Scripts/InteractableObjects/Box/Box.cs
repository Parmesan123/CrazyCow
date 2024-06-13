using Signals;

namespace InteractableObject
{
    public class Box : DestroyBehaviour, ICoinGiver
    {
        public int AmountCoin { get; private set; }

        private void OnEnable()
        {
            AmountCoin = _data.AmountCoin;
        }
        
        public override void Spawn()
        {
            SignalBus.Invoke(new BoxSpawnSignal(this));
            base.Spawn();
        }

        public override void Destroy()
        {
            SignalBus.Invoke(new CoinGiveSignal(transform, AmountCoin));
            
            base.Destroy();
        }
    }
}