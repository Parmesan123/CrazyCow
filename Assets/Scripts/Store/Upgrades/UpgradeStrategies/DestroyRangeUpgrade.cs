public class DestroyRangeUpgrade : IUpgradable
{
    private readonly float _value;

    public DestroyRangeUpgrade(float value)
    {
        _value = value;
    }
    
    public void Upgrade(PlayerData data)
    {
        data.DestroyRange += _value;
    }
}