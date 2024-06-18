using UnityEngine;

public class SaveableSingleton<T>: MonoBehaviour where T: Component
{
    private static T _instance;

    public static T Instance {
        get {
            _instance ??= FindFirstObjectByType<T>();
            if (_instance != null) 
                return _instance;
            
            GameObject obj = new GameObject()
            {
                name = typeof(T).Name + "AutoCreated"
            };
            _instance = obj.AddComponent<T>();

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton() {
        if (!Application.isPlaying) {
            return;
        }

        transform.SetParent(null);

        if (_instance == null) {
            _instance = this as T;
            DontDestroyOnLoad(transform.gameObject);
            enabled = true;
        } else {
            if (this != _instance) {
                Destroy(gameObject);
            }
        }
    }
}