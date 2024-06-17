namespace Entities
{
    public interface IPowerUp
    {
        public float Time { get; }
    
        public void Do(PlayerBehavior powerUpable);

        public void Undo(PlayerBehavior powerUpable);
    }
}