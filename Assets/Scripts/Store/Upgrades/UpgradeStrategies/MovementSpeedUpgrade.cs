public class MovementSpeedUpgrade : IUpgradable
{
    private readonly float _value;

    public MovementSpeedUpgrade(float value)
    {
        _value = value;
    }
    
    public void Upgrade(PlayerData data)
    {
        data.MovementSpeed += _value;
    }
}