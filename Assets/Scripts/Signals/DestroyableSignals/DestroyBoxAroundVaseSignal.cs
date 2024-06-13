﻿using InteractableObject;
using Services;

namespace Signals
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