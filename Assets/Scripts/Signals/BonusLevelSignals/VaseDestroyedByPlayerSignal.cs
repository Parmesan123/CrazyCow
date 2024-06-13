using InteractableObject;
using Services;

namespace Signals
{
    public class VaseDestroyedByPlayerSignal : ISignal
    {
        public readonly Vase Vase;

        public VaseDestroyedByPlayerSignal(Vase vase)
        {
            Vase = vase;
        }
    }
}