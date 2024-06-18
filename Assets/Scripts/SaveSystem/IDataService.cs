public interface IDataService
{
    public void Save(GameData data);
    public GameData Load(string name);
}