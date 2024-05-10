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

    }
    private void Start()
    {
        GameLoopManager.Instance.OnStateChanged += LevelManager_OnStateChanged;
        Hide();
    }

    private void LevelManager_OnStateChanged(object sender, GameLoopManager.OnStateChangedEventArgs e)
    {
        if (GameLoopManager.Instance.IsRespawn())
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

    private void AddSpawnForLevel(LevelEntity levelEntity)
    {
        GameLoopManager.Instance.SpawnPlace(levelEntity, blackTransform);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
