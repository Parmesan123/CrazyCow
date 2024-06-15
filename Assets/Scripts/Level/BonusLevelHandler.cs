using System;
using System.Collections.Generic;
using Entities;
using Handlers;
using Level;
using ModestTree;
using NaughtyAttributes;
using Unity.AI.Navigation;
using UnityEngine;
using Zenject;

public class BonusLevelHandler : BaseLevelHandler
{
    [SerializeField, Expandable] private BonusLevelData _levelData;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private BotBehavior _bot;
    [SerializeField] private BoxCollider _levelBounds;
    [SerializeField] private Portal _bonusLevelPortal;

    public IReadOnlyList<Vase> Vases => _vasesOnField;

    private NavMeshSurface _surface;
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
    }

    private void Awake()
    {
        _portalFactory.OnPortalEnterEvent += StartBonusLevel;
        foreach (Portal spawnedPortal in _portalFactory.SpawnedPortals)
            spawnedPortal.OnEnter += StartBonusLevel;

        _bonusLevelPortal.OnEnter += EndBonusLevel;

        _surface = GetComponent<NavMeshSurface>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        _portalFactory.OnPortalEnterEvent -= StartBonusLevel;
        
        _bonusLevelPortal.OnEnter -= EndBonusLevel;
    }

    public override void Unpause()
    {
        //TODO: rework
        _portalFactory.OnPortalEnterEvent -= StartBonusLevel;
    }

    public override void Pause()
    {
        //TODO: rework
        _portalFactory.OnPortalEnterEvent -= StartBonusLevel;
    }
    
    private void StartBonusLevel()
    {
        _currentBotPoints = 0;
        
        _player.transform.position = _playerSpawnPoint.position;
        _currentPlayerPoints = 0;

        for (int i = 0; i < _levelData.VaseCount; i++)
        {
            Vase newVase = _spawnHandler.TrySpawnAndPlaceEntity<Vase>(_levelBounds);
            
            foreach (Box vaseBox in newVase.Boxes)
            {
                vaseBox.OnDestroyEvent += EntityDestroyed;
                _boxesOnField.Add(vaseBox);
            }
            
            newVase.OnDestroyEvent += EntityDestroyed;
            _vasesOnField.Add(newVase);
        }
        
        for (int i = 0; i < _levelData.BoxCount; i++)
        {
            Box newBox = _spawnHandler.TrySpawnAndPlaceEntity<Box>(_levelBounds);
            
            newBox.OnDestroyEvent += EntityDestroyed;
            _boxesOnField.Add(newBox);
        }
        
        _surface.BuildNavMesh();
        
        _bot.GetComponent<BoxDestroyer>().OnDestroyEvent += VaseDestroyedByBot;
        _bot.gameObject.SetActive(true);
        
        _player.GetComponent<BoxDestroyer>().OnDestroyEvent += VaseDestroyedByPlayer;
        return;
        
        void EntityDestroyed(IDestroyable destroyable)
        {
            destroyable.OnDestroyEvent -= EntityDestroyed;
            _surface.BuildNavMesh();

            if (destroyable is Box box)
            {
                
                _boxesOnField.Remove(box);
            }
        }
    }

    private void EndBonusLevel()
    {
        _bot.GetComponent<BoxDestroyer>().OnDestroyEvent -= VaseDestroyedByBot;
        _player.GetComponent<BoxDestroyer>().OnDestroyEvent -= VaseDestroyedByPlayer;
        
        _bonusLevelPortal.gameObject.SetActive(true);
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

    private void CalculateWin()
    {
        if (!_vasesOnField.IsEmpty())
            return;

        Debug.Log(_currentBotPoints > _currentPlayerPoints ? "Bot win" : "Player win");
    }
}