public class DestroySpeedUpgrade : IUpgradable
{
    private readonly float _value;

    public DestroySpeedUpgrade(float value)
    {
        _value = value;
    }
    
    public void Upgrade(PlayerData data)
    {
        data.DestroyBonusTime += _value;
    }
}