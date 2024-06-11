using InteractableObject;
using Services;
using UnityEngine;

namespace Signals
{
    public class DestroyAnimationSignal : ISignal
    {
        public DestroyableData Data;
        public GameObject Model;

        public DestroyAnimationSignal(DestroyableData data, GameObject model)
        {
            Data = data;
            Model = model;
        }
    }
}
