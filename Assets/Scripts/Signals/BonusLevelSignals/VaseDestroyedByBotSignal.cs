﻿using InteractableObject;
using Services;

namespace Signals
{
    public class VaseDestroyedByBotSignal : ISignal
    {
        public readonly Vase Vase;

        public VaseDestroyedByBotSignal(Vase vase)
        {
            Vase = vase;
        }
    }
}