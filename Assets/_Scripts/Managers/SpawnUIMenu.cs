using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUIMenu : MonoBehaviour
{

    [SerializeField] private Button pinkButton;
    [SerializeField] private Button purpleButton;
    [SerializeField] private Button blackButton;

    [SerializeField] private Transform pinkTransform;
    [SerializeField] private Transform purpleTransform;
    [SerializeField] private Transform blackTransform;

    private void Awake()
    {
        pinkButton.onClick.AddListener(() =>
        {
            LevelManager.Instance.SpawnPlace(LevelManager.LevelColor.Pink, pinkTransform);
            Hide();
        });
        purpleButton.onClick.AddListener(() =>
        {
            LevelManager.Instance.SpawnPlace(LevelManager.LevelColor.Purple, purpleTransform);
            Hide();
        });
        blackButton.onClick.AddListener(() =>
        {
            LevelManager.Instance.SpawnPlace(LevelManager.LevelColor.Black, blackTransform);
            Hide();
        });
    }
    private void Start()
    {
        LevelManager.Instance.OnStateChanged += LevelManager_OnStateChanged;
        Hide();
    }

    private void LevelManager_OnStateChanged(object sender, LevelManager.OnStateChangedEventArgs e)
    {
        if (LevelManager.Instance.IsRespawn())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
