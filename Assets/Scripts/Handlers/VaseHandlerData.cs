﻿using UnityEngine;

namespace InteractableObject
{
    [CreateAssetMenu(fileName = "VaseData", menuName = "ScriptableObjects/Data/VaseData")]
    public class VaseHandlerData : ScriptableObject
    {
        [field: SerializeField] public float SeekingRadius { get; private set; }
    }
}