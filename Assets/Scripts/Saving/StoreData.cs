using System;
using System.Collections.Generic;
using Skills;
using Store;

namespace Saving
{   
    [Serializable]
    public class StoreData
    {
        public List<int> UpgradesData;
        private List<StoreUpgradeSlotUI> _upgrades;
    
        public List<bool> SkillsData;
        private List<SkillSlotUI> _skills;
        
        public StoreData(List<StoreUpgradeSlotUI> upgradables, List<SkillSlotUI> skills)
        {
            UpgradesData = new List<int>();
            SkillsData = new List<bool>();
            
            _upgrades = new List<StoreUpgradeSlotUI>(upgradables);
            foreach (StoreUpgradeSlotUI _ in upgradables)
                UpgradesData.Add(0);
    
            _skills = new List<SkillSlotUI>(skills);
            foreach (SkillSlotUI _ in _skills)
                SkillsData.Add(false);
        }
    
        public void UpdateSlots(List<StoreUpgradeSlotUI> upgradables, List<SkillSlotUI> skills)
        {
            _upgrades = new List<StoreUpgradeSlotUI>(upgradables);
    
            _skills = new List<SkillSlotUI>(skills);
        }
    
        public void UpdateUpgradeData(IUpgradable upgradable)
        {
            int i = 0;
            foreach (StoreUpgradeSlotUI slotUI in _upgrades)
            {
                if (slotUI.Upgradable == upgradable)
                {
                    UpgradesData[i] += 1;
                    break;
                }
    
                ++i;
            }
        }
    
        public void EnableSkill(ISkill skill)
        {
            int i = 0;
            foreach (SkillSlotUI slotUI in _skills)
            {
                if (slotUI.Skill == skill)
                {
                    SkillsData[i] = true;
                    break;
                }
    
                ++i;
            }
        }
    }
}