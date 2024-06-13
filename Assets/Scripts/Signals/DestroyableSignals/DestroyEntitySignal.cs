using Services;

namespace Signals
{
    public class DestroyEntitySignal : ISignal
    {
        public readonly IDestroyable Entity;

        public DestroyEntitySignal(IDestroyable entity)
        {
            Entity = entity;
        }
    }
}