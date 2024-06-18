using System.IO;
using Application = UnityEngine.Device.Application;

public class FileDataService : IDataService
{
    private readonly ISerializer _serializer;
    private readonly string _dataPath;
    private readonly string _dataFileName;

    private string SavePath => Path.Combine(_dataPath, _dataFileName);

    public FileDataService(ISerializer serializer)
    {
        _serializer = serializer;
        _dataPath = Application.persistentDataPath;
        _dataFileName = "save.json";
    }
    
    public void Save(GameData data)
    {
        string file = SavePath;
        
        File.WriteAllText(file, _serializer.Serialize(data));
    }

    public GameData Load(string name)
    {
        string file = SavePath;
        
        if (!File.Exists(name))
            return new GameData();
        
        GameData gameData = _serializer.Deserialize<GameData>(File.ReadAllText(file));
        return gameData;
    }
}