using System;
using System.Collections.Generic;
using Handlers;
using InteractableObject;
using Level;
using ModestTree;
using NaughtyAttributes;
using Player;
using UnityEngine;
using Zenject;

public class BonusLevelHandler : BaseLevelHandler
{
    [SerializeField, Expandable] private BonusLevelData _levelData;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform _botSpawnPoint;
    [SerializeField] private BoxCollider _levelBounds;

    private PortalFactory _portalFactory;
    private List<Vase> _vasesOnField;
    private List<Box> _boxesOnField;
    private int _currentBotPoints;
    private int _currentPlayerPoints;
    
    [Inject]
    protected void Construct(PauseHandler pauseHandler, SpawnHandler spawnHandler, PlayerMovement player, PortalFactory portalFactory)
    {
        base.Construct(pauseHandler, spawnHandler, player);

        _vasesOnField = new List<Vase>();
        _boxesOnField = new List<Box>();

        _portalFactory = portalFactory;
        _portalFactory.OnPortalEnterEvent += PortalEntered;
        foreach (Portal spawnedPortal in _portalFactory.SpawnedPortals)
            spawnedPortal.OnEnter += PortalEntered;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        _portalFactory.OnPortalEnterEvent -= PortalEntered;
    }

    public override void Unpause()
    {
        //TODO: rework
        _portalFactory.OnPortalEnterEvent -= PortalEntered;
    }

    public override void Pause()
    {
        //TODO: rework
        _portalFactory.OnPortalEnterEvent -= PortalEntered;
    }
    
    private void VaseDestroyedByBot(Vase vase)
    {
        _currentBotPoints++;

        if (!_vasesOnField.Contains(vase))
            throw new Exception("Can't process vase destroy signal");
        _vasesOnField.Remove(vase);
        
        CalculateWin();
    }

    private void VaseDestroyedByPlayer(Vase vase)
    {
        _currentPlayerPoints++;
        
        if (!_vasesOnField.Contains(vase))
            throw new Exception("Can't process vase destroy signal");
        _vasesOnField.Remove(vase);
        
        CalculateWin();
    }
    
    private void PortalEntered()
    {
        _currentBotPoints = 0;
        
        _player.transform.position = _playerSpawnPoint.position;
        _currentPlayerPoints = 0;

        for (int i = 0; i < _levelData.BoxCount; i++)
        {
            Box newBox = _spawnHandler.TrySpawnAndPlaceEntity<Box>(_levelBounds);
            _boxesOnField.Add(newBox);
        }

        for (int i = 0; i < _levelData.VaseCount; i++)
        {
            Vase newVase = _spawnHandler.TrySpawnAndPlaceEntity<Vase>(_levelBounds);
            _vasesOnField.Add(newVase);
        }
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