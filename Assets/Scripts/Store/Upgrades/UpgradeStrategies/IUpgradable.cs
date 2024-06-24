using Saving;

namespace Store
{
    public interface IUpgradable
    {
        public void Upgrade(PlayerData data);
    }
}