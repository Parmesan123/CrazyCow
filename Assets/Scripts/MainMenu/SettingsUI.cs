using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private Button _closeSettingsButton;

        private void Awake()
        {
            _closeSettingsButton.onClick.AddListener(CloseSettings);
        }

        private void CloseSettings()
        {
            gameObject.SetActive(false);
        }
    }
}