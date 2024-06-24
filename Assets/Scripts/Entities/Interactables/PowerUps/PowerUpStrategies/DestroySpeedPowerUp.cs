namespace Entities
{
    public class DestroySpeedPowerUp : IPowerUp
    {
        private const float VALUE = 0.5f;
    
        public float Time { get; }

        public DestroySpeedPowerUp(float time)
        {
            Time = time;
        }
    
        public void Do(PlayerBehavior powerUpable)
        {
            BoxDestroyer boxDestroyer = powerUpable.PlayerBoxDestroyer;

            boxDestroyer.DestroyBonusTime += VALUE;
        }

        public void Undo(PlayerBehavior powerUpable)
        {
            BoxDestroyer boxDestroyer = powerUpable.PlayerBoxDestroyer;

            boxDestroyer.DestroyBonusTime -= VALUE;
        }
    }
}