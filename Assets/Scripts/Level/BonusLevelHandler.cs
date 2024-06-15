using System;
using System.Collections.Generic;
using Bot;
using Handlers;
using InteractableObject;
using Level;
using ModestTree;
using NaughtyAttributes;
using Player;
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
    
    private void StartBonusLevel()
    {
        _currentBotPoints = 0;
        
        _player.transform.position = _playerSpawnPoint.position;
        _currentPlayerPoints = 0;

        for (int i = 0; i < _levelData.VaseCount; i++)
        {
            Vase newVase = _spawnHandler.TrySpawnAndPlaceEntity<Vase>(_levelBounds);
            newVase.OnDestroyEvent += UpdateNavMesh;
            
            foreach (Box vaseBox in newVase.Boxes)
            {
                vaseBox.OnDestroyEvent += UpdateNavMesh;
                _boxesOnField.Add(vaseBox);
            }
            
            _vasesOnField.Add(newVase);
        }
        
        for (int i = 0; i < _levelData.BoxCount; i++)
        {
            Box newBox = _spawnHandler.TrySpawnAndPlaceEntity<Box>(_levelBounds);
            newBox.OnDestroyEvent += UpdateNavMesh;
            _boxesOnField.Add(newBox);
        }
        
        _surface.BuildNavMesh();
        _bot.gameObject.SetActive(true);
        return;
        
        void UpdateNavMesh(IDestroyable destroyable)
        {
            destroyable.OnDestroyEvent -= UpdateNavMesh;
            _surface.BuildNavMesh();
        }
    }

    private void EndBonusLevel()
    {
        _bot.gameObject.SetActive(false);
        _bonusLevelPortal.gameObject.SetActive(true);
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