using System;
using System.Collections.Generic;
using Entities;
using Handlers;
using Level;
using ModestTree;
using NaughtyAttributes;
using UI;
using Unity.AI.Navigation;
using UnityEngine;
using Zenject;

public class BonusLevelHandler : BaseLevelHandler, ICoinGiver
{
    [SerializeField, Expandable] private BonusLevelData _levelData;
    [SerializeField] private Transform _playerStartLevelSpawnPoint;
    [SerializeField] private Transform _playerEndLevelSpawnPoint;
    [SerializeField] private Transform _botStartLevelSpawnPoint;
    [SerializeField] private BotBehavior _bot;
    [SerializeField] private Portal _bonusLevelPortal;
    
    public event Action<ICoinGiver> OnCoinGiveEvent;
    public int AmountCoin => _totalCoins;
    public Transform Transform => transform;

    public event Action OnBonusLevelStarted;
    public event Action OnBonusLevelEnded;
    public event Action<int> OnPlayerScoreUpdateUIEvent;
    public event Action<int> OnBotScoreUpdateUIEvent;
    public IReadOnlyList<Vase> Vases => _vasesOnField;

    private NavMeshSurface _surface;
    private PortalFactory _portalFactory;
    private int _currentBotPoints;
    private int _currentPlayerPoints;
    private int _totalCoins;
    
    [Inject]
    protected void Construct(SpawnHandler spawnHandler, PlayerBehavior player, PortalFactory portalFactory, CoinSpawner coinSpawner)
    {
        base.Construct(spawnHandler, coinSpawner, player);

        _portalFactory = portalFactory;
    }

    protected override void Awake()
    {
        base.Awake();
        
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
    
    private void StartBonusLevel()
    {
        OnBonusLevelStarted?.Invoke();
        _player.transform.position = _playerStartLevelSpawnPoint.position;
        _bot.transform.position = _botStartLevelSpawnPoint.position;
        _coinSpawner.Register(this);

        for (int i = 0; i < _levelData.VaseCount; i++)
        {
            Vase newVase = EntitySpawn<Vase>();
            newVase.OnDestroyEvent += EntityDestroyed;
            
            foreach (Box vaseBox in newVase.Boxes)
                vaseBox.OnDestroyEvent += EntityDestroyed;
        }
        
        for (int i = 0; i < _levelData.BoxCount; i++)
        {
            Box newBox = EntitySpawn<Box>();
            
            newBox.OnDestroyEvent += EntityDestroyed;
        }
        
        _surface.BuildNavMesh();
        
        _bot.GetComponent<BoxDestroyer>().OnDestroyEvent += VaseDestroyedByBot;
        _bot.gameObject.SetActive(true);
        
        _player.PlayerBoxDestroyer.OnDestroyEvent += VaseDestroyedByPlayer;
        return;
        
        void EntityDestroyed(IDestroyable destroyable)
        {
            destroyable.OnDestroyEvent -= EntityDestroyed;
            _surface.BuildNavMesh();
        }
    }

    private void EndBonusLevel()
    {
        OnBonusLevelEnded?.Invoke();
        _player.transform.position = _playerEndLevelSpawnPoint.position;
        
        _bot.GetComponent<BoxDestroyer>().OnDestroyEvent -= VaseDestroyedByBot;
        _player.PlayerBoxDestroyer.OnDestroyEvent -= VaseDestroyedByPlayer;
        
        foreach (Box box in _boxesOnField)
            box.gameObject.SetActive(false);
        _boxesOnField.Clear();
        
        if (_currentPlayerPoints > _currentBotPoints)
            OnCoinGiveEvent.Invoke(this);
        
        _currentBotPoints = 0;
        OnBotScoreUpdateUIEvent?.Invoke(_currentBotPoints);
        _currentPlayerPoints = 0;
        OnPlayerScoreUpdateUIEvent?.Invoke(_currentPlayerPoints);

        _totalCoins = 0;
    }
    
    private void VaseDestroyedByBot(Vase vase)
    {
        if (vase.Boxes.IsEmpty())
        {
            _currentBotPoints++;
            OnBotScoreUpdateUIEvent?.Invoke(_currentBotPoints);
        }
        
        CalculateWin();
    }

    private void VaseDestroyedByPlayer(Vase vase)
    {
        _totalCoins += vase.AmountCoin;
        if (vase.Boxes.IsEmpty())
        {
            _currentPlayerPoints++;
            OnPlayerScoreUpdateUIEvent?.Invoke(_currentPlayerPoints);
        }
        
        CalculateWin();
    }

    private void CalculateWin()
    {
        if (!_vasesOnField.IsEmpty())
            return;

        _totalCoins *= 2;
        
        _bonusLevelPortal.gameObject.SetActive(true);
    }
}