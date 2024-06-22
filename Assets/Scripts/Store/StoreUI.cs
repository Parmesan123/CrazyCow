using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private Button _closeStoreButton;
    [SerializeField] private List<StoreUpgradeSlotUI> _upgradeSlots;

    private UpgradeHandler _upgradeHandler;
    private SaveHandler _saveHandler;
    
    [Inject]
    private void Construct(UpgradeHandler upgradeHandler, SaveHandler saveHandler)
    {
        _upgradeHandler = upgradeHandler;

        _saveHandler = saveHandler;
    }
    
    private void Awake()
    {
        _saveHandler.SaveData.StoreData ??= new StoreData(_upgradeSlots);
        _saveHandler.SaveData.StoreData.UpdateSlots(_upgradeSlots);
        _closeStoreButton.onClick.AddListener(CloseStore);
        
        int i = 0;
        StoreData data = _saveHandler.SaveData.StoreData;
        foreach (StoreUpgradeSlotUI slotUI in _upgradeSlots)
        {
            slotUI.OnUpgradePerformed += OnLevelUp;
            slotUI.DefineStrategy(_upgradeHandler.Upgradables[i], data.UpgradableData[i]);
            ++i;
        }
    }

    private void OnDestroy()
    {
        foreach (StoreUpgradeSlotUI slotUI in _upgradeSlots)
            slotUI.OnUpgradePerformed -= OnLevelUp;
    }

    private void OnLevelUp(IUpgradable strategy)
    {
        _upgradeHandler.UpgradePerform(strategy);
        _saveHandler.SaveData.StoreData.UpdateData(strategy);
    }

    private void CloseStore()
    {
        gameObject.SetActive(false);
    }
}