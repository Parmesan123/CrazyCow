using System.Collections;
using Entities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Button _skillButton;
    [SerializeField] private Slider _timeSlider;

    private ISkill _skillStrategy;
    private Transform _playerTransform;
    
    [Inject]
    private void Construct(PlayerBehavior player)
    {
        _playerTransform = player.transform;
    }

    private void Awake()
    {
        _skillButton.onClick.AddListener(OnSkillButtonPressed);

        _timeSlider.value = 0;
    }

    private void OnDestroy()
    {
        _skillButton.onClick.RemoveListener(OnSkillButtonPressed);
    }

    public void DefineStrategy(ISkill strategy)
    {
        _skillStrategy = strategy;
    }

    private void OnSkillButtonPressed()
    {
        _skillStrategy.Perform(_playerTransform);
        _skillButton.interactable = false;
        StartCoroutine(CooldownTimer());

        IEnumerator CooldownTimer()
        {
            WaitForFixedUpdate tick = new WaitForFixedUpdate();
            float time = _skillStrategy.Data.Cooldown;
            _timeSlider.maxValue = time;
            _timeSlider.value = time;
            
            for (; time >= 0;)
            {
                yield return tick;
                
                time -= Time.fixedDeltaTime;
                _timeSlider.value = time;
            }

            _skillButton.interactable = true;
        }
    }
}