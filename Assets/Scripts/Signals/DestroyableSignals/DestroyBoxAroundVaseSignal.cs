using InteractableObject;

namespace Services
{
    public class DestroyBoxAroundVaseSignal : ISignal
    {
        public readonly Box Box;

        public DestroyBoxAroundVaseSignal(Box box)
        {
            Box = box;
        }
    }
}