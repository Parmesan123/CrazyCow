﻿using System.Collections.Generic;
using Saving;
using Skills;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Store
{
    public class StoreUI : MonoBehaviour
{
    [SerializeField] private Button _closeStoreButton;
    [SerializeField] private List<StoreUpgradeSlotUI> _upgradeSlots;
    [SerializeField] private List<SkillSlotUI> _skillSlots;

    private UpgradeProvider _upgradeProvider;
    private SkillProvider _skillProvider;
    private SaveHandler _saveHandler;
    
    [Inject]
    private void Construct(UpgradeProvider upgradeProvider, SkillProvider skillProvider, SaveHandler saveHandler)
    {
        _upgradeProvider = upgradeProvider;

        _skillProvider = skillProvider;

        _saveHandler = saveHandler;
    }
    
    private void Awake()
    {
        _saveHandler.SaveData.StoreData ??= new StoreData(_upgradeSlots, _skillSlots);
        _saveHandler.SaveData.StoreData.UpdateSlots(_upgradeSlots, _skillSlots);
        _closeStoreButton.onClick.AddListener(CloseStore);
        
        int i = 0;
        StoreData data = _saveHandler.SaveData.StoreData;
        foreach (StoreUpgradeSlotUI slotUI in _upgradeSlots)
        {
            slotUI.OnUpgradePerformed += OnUpgradeLevelUp;
            slotUI.DefineStrategy(_upgradeProvider.Upgradables[i], data.UpgradesData[i]);
            ++i;
        }

        i = 0;
        foreach (SkillSlotUI slotUI in _skillSlots)
        {
            slotUI.OnBuyPerformed += OnSkillBuy;
            slotUI.DefineStrategy(_skillProvider.Skills[i], data.SkillsData[i]);
            ++i;
        }
    }

    private void OnDestroy()
    {
        foreach (StoreUpgradeSlotUI slotUI in _upgradeSlots)
            slotUI.OnUpgradePerformed -= OnUpgradeLevelUp;
        
        foreach (SkillSlotUI slotUI in _skillSlots)
            slotUI.OnBuyPerformed -= OnSkillBuy;
    }

    private void OnUpgradeLevelUp(IUpgradable strategy)
    {
        _saveHandler.SaveData.StoreData.UpdateUpgradeData(strategy);
        strategy.Upgrade(_saveHandler.SaveData.PlayerData);
        _saveHandler.Save();
    }

    private void OnSkillBuy(ISkill strategy)
    {
        _saveHandler.Save();
        _saveHandler.SaveData.StoreData.EnableSkill(strategy);
    } 

    private void CloseStore()
    {
        gameObject.SetActive(false);
    }
}
}