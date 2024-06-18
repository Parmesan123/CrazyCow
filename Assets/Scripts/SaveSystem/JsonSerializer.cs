using UnityEngine;

public class JsonSerializer : ISerializer
{
    public string Serialize<T>(T obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public T Deserialize<T>(string name)
    {
        return JsonUtility.FromJson<T>(name);
    }
}