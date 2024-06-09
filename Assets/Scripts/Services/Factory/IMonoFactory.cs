using UnityEngine;

public interface IMonoFactory<out T> where T: MonoBehaviour
{
    public T CreateObject();
}