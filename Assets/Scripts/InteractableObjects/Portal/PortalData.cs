using InteractableObject;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "PortalData", menuName = "ScriptableObjects/Data/PortalData")]
public class PortalData : InteractableData
{
    [SerializeField, MinMaxSlider(5, 10)] private Vector2 _timeUntilDespawn;
    public float TimeUntilDespawn => Random.Range(_timeUntilDespawn.x, _timeUntilDespawn.y);
}