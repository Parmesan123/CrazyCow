using UnityEngine;

public interface ISkill
{
    public SkillData Data { get; }
    
    public void Perform(Transform player);
}