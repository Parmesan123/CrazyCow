using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillProvider
{
    public IReadOnlyList<ISkill> Skills => _skillSet;
    
    private readonly List<ISkill> _skillSet;

    private SkillProvider()
    {
        SkillData firstBoxSkillData = Resources.Load<SkillData>("SO/Skills/FirstBoxDestroy");
        SkillData allBoxSkillData = Resources.Load<SkillData>("SO/Skills/AllBoxDestroy");

        _skillSet = new List<ISkill>()
        {
            new DestroyFirstBoxSkill(firstBoxSkillData),
            new DestroyAllBoxSkill(allBoxSkillData)
        };
    }

    public ISkill GetSkill<T>() where T: ISkill
    {
        ISkill skill = _skillSet.FirstOrDefault(s => s is T);
        
        return skill;
    }
}