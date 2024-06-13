﻿using Services;
using UnityEngine;

namespace Signals
{
    public class TouchPerformedSignal : ISignal
    {
        public readonly bool IsTouched;
        public readonly Vector2 TouchPosition;

        public TouchPerformedSignal(bool isTouched, Vector2 touchPosition)
        {
            IsTouched = isTouched;
            TouchPosition = touchPosition;
        }
    }
}