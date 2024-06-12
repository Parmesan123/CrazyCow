﻿using System;
using System.Collections.Generic;
using Handlers;
using InteractableObject;
using Level;
using ModestTree;
using NaughtyAttributes;
using Player;
using Services;
using UnityEngine;
using Zenject;

public class BonusLevelHandler : BaseLevelHandler, ISignalReceiver<VaseDestroyedByBotSignal>, ISignalReceiver<VaseDestroyedByPlayerSignal>
{
    [SerializeField, Expandable] private BonusLevelData _levelData;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform _botSpawnPoint;
    [SerializeField] private BoxCollider _levelBounds;

    private List<Vase> _vasesOnField;
    private int _currentBotPoints;
    private int _currentPlayerPoints;
    
    [Inject]
    protected override void Construct(SpawnHandler spawnHandler, PlayerMovement player, SignalBus signalBus, PauseHandler pauseHandler)
    {
        base.Construct(spawnHandler, player, signalBus, pauseHandler);

        _vasesOnField = new List<Vase>();
    }

    public override void Unpause()
    {
        _signalBus.Register<VaseDestroyedByBotSignal>(this);
        _signalBus.Register<VaseDestroyedByPlayerSignal>(this);
    }

    public override void Pause()
    {
        _signalBus.Unregister<VaseDestroyedByBotSignal>(this);
        _signalBus.Unregister<VaseDestroyedByPlayerSignal>(this);
    }

    public void StartNewBonusLevel()
    {
        //TODO: add bot spawn
        _currentBotPoints = 0;
        
        _player.transform.position = _playerSpawnPoint.position;
        _currentPlayerPoints = 0;

        int generatedBoxCount = _levelData.BoxCount;
        for (int i = 0; i < generatedBoxCount; i++)
            _spawnHandler.TrySpawnAndPlaceEntity<Box>(_levelBounds);

        for (int i = 0; i < _levelData.VaseCount; i++)
        {
            Vase newVase = _spawnHandler.TrySpawnAndPlaceEntity<Vase>(_levelBounds);
            _vasesOnField.Add(newVase);
        }
    }
    
    public void Receive(VaseDestroyedByBotSignal signal)
    {
        _currentBotPoints++;

        if (!_vasesOnField.Contains(signal.Vase))
            throw new Exception("Can't process vase destroy signal");
        _vasesOnField.Remove(signal.Vase);
        
        CalculateWin();
    }

    public void Receive(VaseDestroyedByPlayerSignal signal)
    {
        _currentPlayerPoints++;
        
        if (!_vasesOnField.Contains(signal.Vase))
            throw new Exception("Can't process vase destroy signal");
        _vasesOnField.Remove(signal.Vase);
        
        CalculateWin();
    }

    private void CalculateWin()
    {
        if (!_vasesOnField.IsEmpty())
            return;
        
        if (_currentBotPoints > _currentPlayerPoints)
            Debug.Log("Bot win");
        else
            Debug.Log("Player win");
    }
}