using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MenuWalletUI : MonoBehaviour
{
    [SerializeField] private MenuWalletHandler _menuWalletHandler;
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        _menuWalletHandler.OnUIUpdateEvent += UpdateGameWalletUI;
    }

    private void OnDestroy()
    {
        _menuWalletHandler.OnUIUpdateEvent -= UpdateGameWalletUI;
    }

    private void UpdateGameWalletUI(int coins)
    {
        _text.text = coins.ToString();
        _text.transform.localScale = Vector3.one;
        _text.transform.DOKill();
        _text.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f, 1);
    }
}