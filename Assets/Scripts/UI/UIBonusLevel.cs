using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class UIBonusLevel : MonoBehaviour
{
    [SerializeField] private GameObject _scoreContainer;
    [SerializeField] private TextMeshProUGUI _playerScore;
    [SerializeField] private TextMeshProUGUI _botScore;

    private BonusLevelHandler _bonusLevelHandler;
    
    [Inject]
    private void Construct(BonusLevelHandler bonusLevelHandler)
    {
        _bonusLevelHandler = bonusLevelHandler;
    }

    private void Awake()
    {
        UpdateBotScore(0);
        UpdatePlayerScore(0);
        
        _bonusLevelHandler.OnBotScoreUpdateUIEvent += UpdateBotScore;
        _bonusLevelHandler.OnPlayerScoreUpdateUIEvent += UpdatePlayerScore;
        _bonusLevelHandler.OnBonusLevelStarted += EnableScoreContainer;
        _bonusLevelHandler.OnBonusLevelEnded += DisableScoreContainer;
    }

    private void OnDestroy()
    {
        _bonusLevelHandler.OnBotScoreUpdateUIEvent -= UpdateBotScore;
        _bonusLevelHandler.OnPlayerScoreUpdateUIEvent -= UpdatePlayerScore;
        _bonusLevelHandler.OnBonusLevelStarted -= EnableScoreContainer;
        _bonusLevelHandler.OnBonusLevelEnded -= DisableScoreContainer;
    }

    private void UpdatePlayerScore(int score)
    {
        _playerScore.text = score.ToString();
        _playerScore.transform.localScale = Vector3.one;
        _playerScore.transform.DOKill();
        _playerScore.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f, 1);
    }

    private void UpdateBotScore(int score)
    {
        _botScore.text = score.ToString();
        _botScore.transform.localScale = Vector3.one;
        _botScore.transform.DOKill();
        _botScore.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f, 1);
    }

    private void EnableScoreContainer()
    {
        _scoreContainer.SetActive(true);
    }

    private void DisableScoreContainer()
    {
        _scoreContainer.SetActive(false);
    }
}