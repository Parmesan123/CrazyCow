using System.Collections.Generic;
using Saving;
using UnityEngine;
using Zenject;

namespace Skills
{
    public class SkillBuilder : MonoBehaviour
    {
        [SerializeField] private SkillUI _firstBoxSkillUI;
        [SerializeField] private SkillUI _allBoxSkillUI;
    
        private List<SkillUI> _activeSkills;
        
        private SkillProvider _provider;
        private BonusLevelHandler _bonusLevelHandler;
    
        private StoreData _storeData;
        
        [Inject]
        private void Construct(SkillProvider provider, BonusLevelHandler bonusLevel, SaveHandler saveHandler)
        {
            _provider = provider;
    
            _bonusLevelHandler = bonusLevel;
            _bonusLevelHandler.OnBonusLevelStarted += DisableSkills;
            _bonusLevelHandler.OnBonusLevelEnded += EnableSkills;
    
            _storeData = saveHandler.SaveData.StoreData;
        }
    
        private void Awake()
        {
            _activeSkills = new List<SkillUI>();
            
            _firstBoxSkillUI.DefineStrategy(_provider.GetSkill<DestroyFirstBoxSkill>());
            _firstBoxSkillUI.gameObject.SetActive(false);
            if (_storeData.SkillsData[0])
                _activeSkills.Add(_firstBoxSkillUI);
            
            _allBoxSkillUI.DefineStrategy(_provider.GetSkill<DestroyAllBoxSkill>());
            _allBoxSkillUI.gameObject.SetActive(false);
            if (_storeData.SkillsData[1])
                _activeSkills.Add(_allBoxSkillUI);
            
            EnableSkills();
        }
    
        private void OnDestroy()
        {
            _bonusLevelHandler.OnBonusLevelStarted -= DisableSkills;
            _bonusLevelHandler.OnBonusLevelEnded -= EnableSkills;
        }
    
        private void EnableSkills()
        {
            foreach (SkillUI activeSkill in _activeSkills)
                activeSkill.gameObject.SetActive(true);
        }
    
        private void DisableSkills()
        {
            foreach (SkillUI activeSkill in _activeSkills)
                activeSkill.gameObject.SetActive(false);
        }
    }
}