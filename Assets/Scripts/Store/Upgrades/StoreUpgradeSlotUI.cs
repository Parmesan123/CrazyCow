using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StoreUpgradeSlotUI : MonoBehaviour
{
    [SerializeField] private StoreUpgradeSlotData _data;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Slider _upgradeSlider;
    [SerializeField] private TextMeshProUGUI _upgradeLabel;
    [SerializeField] private TextMeshProUGUI _upgradeCost;

    public event Action<IUpgradable> OnUpgradePerformed;
    public IUpgradable Upgradable => _upgradableStrategy;
    
    private MenuWalletHandler _walletHandler;

    private IUpgradable _upgradableStrategy;
    
    [Inject]
    private void Construct(MenuWalletHandler walletHandler)
    {
        _walletHandler = walletHandler;
    }

    private void Awake()
    {
        _upgradeButton.onClick.AddListener(OnUpgradeButtonPressed);
    }

    public void DefineStrategy(IUpgradable upgradable, int level)
    {
        _upgradableStrategy = upgradable;

        _upgradeLabel.text = _data.Label;
        _upgradeCost.text = _data.Cost.ToString();
        _upgradeSlider.maxValue = _data.MaximumLevel;
        _upgradeSlider.value = level;
    }

    private void OnUpgradeButtonPressed()
    {
        if (_upgradeSlider.value >= _data.MaximumLevel)
            return;
        
        if (!_walletHandler.TrySpend(_data.Cost))
            return;

        _upgradeSlider.value += 1;
        OnUpgradePerformed.Invoke(_upgradableStrategy);
    }
}