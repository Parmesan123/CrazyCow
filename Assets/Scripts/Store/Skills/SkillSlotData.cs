using UnityEngine;

namespace Store
{
    [CreateAssetMenu(fileName = "SkillSlotData", menuName = "ScriptableObjects/Data/SkillSlotData", order = 0)]
    public class SkillSlotData : ScriptableObject
    {
        [field: SerializeField] public string Label { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
    }
}