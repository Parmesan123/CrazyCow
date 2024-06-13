using InteractableObject;
using Services;

namespace Signals
{
    public class BoxSpawnSignal : ISignal
    {
        public readonly Box Box;

        public BoxSpawnSignal(Box box)
        {
            Box = box;
        }
    }
}