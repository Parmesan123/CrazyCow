namespace Entities
{
    public class MovementSpeedPowerUp : IPowerUp
    {
        private const int VALUE = 2;
        
        public float Time { get; }

        public MovementSpeedPowerUp(float time)
        {
            Time = time;
        } 
    
        public void Do(PlayerBehavior powerUpable)
        {
            PlayerMovement playerMovement = powerUpable.PlayerMovement;

            playerMovement.CurrentSpeed += VALUE;
        }

        public void Undo(PlayerBehavior powerUpable)
        {
            PlayerMovement playerMovement = powerUpable.PlayerMovement;

            playerMovement.CurrentSpeed -= VALUE;
        }
    }
}