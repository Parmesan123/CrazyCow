using UnityEngine;

namespace Skills
{
    public interface ISkill
    {
        public SkillData Data { get; }
    
        public void Perform(Transform player);
    }
}