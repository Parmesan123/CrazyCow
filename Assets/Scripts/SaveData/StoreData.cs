using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class StoreData
{
    public List<int> UpgradableData;
    private List<StoreUpgradeSlotUI> slots;
    
    public StoreData(List<StoreUpgradeSlotUI> upgradables)
    {
        UpgradableData = new List<int>();
        slots = new List<StoreUpgradeSlotUI>(upgradables);

        foreach (StoreUpgradeSlotUI upgradable in upgradables)
            UpgradableData.Add(0);
    }

    public void UpdateSlots(List<StoreUpgradeSlotUI> upgradables)
    {
        slots = new List<StoreUpgradeSlotUI>(upgradables);
    }

    public void UpdateData(IUpgradable upgradable)
    {
        int i = 0;
        foreach (StoreUpgradeSlotUI slotUI in slots)
        {
            if (slotUI.Upgradable == upgradable)
            {
                UpgradableData[i] += 1;
                break;
            }

            ++i;
        }
    }
}