using InteractableObject;
using Services;

namespace Signals
{
    public class VaseSpawnSignal : ISignal
    {
        public readonly Vase Vase;

        public VaseSpawnSignal(Vase vase)
        {
            Vase = vase;
        }
    }
}
