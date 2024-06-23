using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/Data/SkillData")]
public class SkillData : ScriptableObject
{
    [field: SerializeField] public float Cooldown { get; private set; }
    [field: SerializeField] public float Radius { get; private set; }
}