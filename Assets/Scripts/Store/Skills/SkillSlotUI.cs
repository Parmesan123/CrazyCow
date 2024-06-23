using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private SkillSlotData _data;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TextMeshProUGUI _upgradeLabel;
    [SerializeField] private TextMeshProUGUI _upgradeCost;

    public event Action<ISkill> OnBuyPerformed;
    public ISkill Skill => _skillStrategy;
    
    private MenuWalletHandler _walletHandler;

    private ISkill _skillStrategy;
    
    [Inject]
    private void Construct(MenuWalletHandler walletHandler)
    {
        _walletHandler = walletHandler;
    }

    private void Awake()
    {
        _upgradeButton.onClick.AddListener(OnUpgradeButtonPressed);
    }

    public void DefineStrategy(ISkill skill, bool enabled)
    {
        _skillStrategy = skill;

        _upgradeLabel.text = _data.Label;
        _upgradeCost.text = _data.Cost.ToString();

        if (!enabled) 
            return;
        
        _upgradeButton.interactable = false;
        _upgradeCost.text = "Sold";
    }

    private void OnUpgradeButtonPressed()
    {
        if (!_walletHandler.TryRemoveCoins(_data.Cost))
            return;

        OnBuyPerformed.Invoke(_skillStrategy);
    }
}