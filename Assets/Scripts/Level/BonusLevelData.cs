using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusLevelData", menuName = "ScriptableObjects/Data/BonusLevelData", order = 0)]
public class BonusLevelData : ScriptableObject
{
    [SerializeField, MinMaxSlider(6, 9)] private Vector2Int _bonusLeveLBoxCount;
    public int BoxCount => Random.Range(_bonusLeveLBoxCount.x, _bonusLeveLBoxCount.y);

    [SerializeField, MinMaxSlider(2, 4)] private Vector2Int _bonusLevelVaseCount;
    public int VaseCount => Random.Range(_bonusLevelVaseCount.x, _bonusLevelVaseCount.y);
}