using Handlers;
using InteractableObject;
using NaughtyAttributes;
using Player;
using Services;
using UnityEngine;
using Zenject;

public class BonusLevelHandler : MonoBehaviour, ISignalReceiver<VaseDestroyedByBotSignal>, ISignalReceiver<VaseDestroyedByPlayerSignal>
{
    [SerializeField, Expandable] private BonusLevelData _levelData;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform _botSpawnPoint;
    [SerializeField] private BoxCollider _levelBounds;

    private SignalBus _signalBus;
    private PlayerMovement _playerMovement;
    private SpawnHandler _spawnHandler;

    private int _currentBotPoints;
    private int _currentPlayerPoints;
    private int _currentVaseCount;
    
    [Inject]
    private void Construct(SignalBus signalBus, PlayerMovement playerMovement, SpawnHandler spawnHandler)
    {
        _signalBus = signalBus;
        _playerMovement = playerMovement;
        _spawnHandler = spawnHandler;
    }

    private void OnEnable()
    {
        _signalBus.Register<VaseDestroyedByBotSignal>(this);
        _signalBus.Register<VaseDestroyedByPlayerSignal>(this);
        
        StartNewBonusLevel();
    }

    private void OnDisable()
    {
        _signalBus.Unregister<VaseDestroyedByBotSignal>(this);
        _signalBus.Unregister<VaseDestroyedByPlayerSignal>(this);
    }

    private void StartNewBonusLevel()
    {
        //TODO: add bot spawn
        _currentBotPoints = 0;
        
        _playerMovement.transform.position = _playerSpawnPoint.position;
        _currentPlayerPoints = 0;

        int generatedBoxCount = _levelData.BoxCount;
        for (int i = 0; i < generatedBoxCount; i++)
            _spawnHandler.TrySpawnAndPlaceEntity<Box>(_levelBounds);
        
        _currentVaseCount = _levelData.VaseCount;
        for (int i = 0; i < _currentVaseCount; i++)
            _spawnHandler.TrySpawnAndPlaceEntity<Vase>(_levelBounds);
    }

    private void CalculateWin()
    {
        if (_currentVaseCount != 0)
            return;
        
        if (_currentBotPoints > _currentPlayerPoints)
            Debug.Log("Bot win");
        else
            Debug.Log("Player win");
    }

    public void Receive(VaseDestroyedByBotSignal signal)
    {
        _currentBotPoints++;
        _currentVaseCount--;
        
        CalculateWin();
    }

    public void Receive(VaseDestroyedByPlayerSignal signal)
    {
        _currentPlayerPoints++;
        _currentVaseCount--;
        
        CalculateWin();
    }
}