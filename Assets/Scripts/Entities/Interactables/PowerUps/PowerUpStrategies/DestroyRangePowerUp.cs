namespace Entities
{
    public class DestroyRangePowerUp : IPowerUp
    {
        private const float VALUE = 1f;
    
        public float Time { get; }

        public DestroyRangePowerUp(float time)
        {
            Time = time;
        }
    
        public void Do(PlayerBehavior powerUpable)
        {
            BoxDestroyer boxDestroyer = powerUpable.PlayerBoxDestroyer;

            float bonusRange = boxDestroyer.DestroyRadius + VALUE;
            boxDestroyer.UpdateRange(bonusRange);
        }

        public void Undo(PlayerBehavior powerUpable)
        {
            BoxDestroyer boxDestroyer = powerUpable.PlayerBoxDestroyer;

            float initialRange = boxDestroyer.DestroyRadius - VALUE;
            boxDestroyer.UpdateRange(initialRange);
        }
    }
}
