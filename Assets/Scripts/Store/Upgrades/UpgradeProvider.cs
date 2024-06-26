﻿using System.Collections.Generic;
using Entities;
using Saving;
using UnityEngine;
using Zenject;

namespace Store
{
    public class UpgradeProvider : MonoBehaviour
    {
        [SerializeField] private CharacterData _playerDefaultData;
        [SerializeField] private UpgradeData _upgradeData;

        public IReadOnlyList<IUpgradable> Upgradables => _upgrades;
    
        private SaveHandler _saveHandler;
        private List<IUpgradable> _upgrades;

        [Inject]
        private void Construct(SaveHandler saveHandler)
        {
            _saveHandler = saveHandler;
        }
    
        private void Awake()
        {
            _upgrades = new List<IUpgradable>()
            {
                new MovementSpeedUpgrade(_upgradeData.MovementSpeedValue),
                new DestroySpeedUpgrade(_upgradeData.DestroySpeedValue),
                new DestroyRangeUpgrade(_upgradeData.DestroyRangeValue)
            };

            _saveHandler.SaveData.PlayerData ??=
                new PlayerData(_playerDefaultData.CharacterSpeed, 0, _playerDefaultData.DestroyRange);
        }
    }
}