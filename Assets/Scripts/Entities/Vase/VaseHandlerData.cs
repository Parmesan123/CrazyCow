using UnityEngine;

[CreateAssetMenu(fileName = "Vase Data", menuName = "SO/Vase Data", order = 0)]
public class VaseHandlerData : ScriptableObject
{
    [field: SerializeField] public float SeekingRadius { get; private set; }
}