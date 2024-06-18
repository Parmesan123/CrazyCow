using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private Button _closeStoreButton;

    private void Awake()
    {
        _closeStoreButton.onClick.AddListener(CloseStore);
    }

    private void CloseStore()
    {
        gameObject.SetActive(false);
    }
}