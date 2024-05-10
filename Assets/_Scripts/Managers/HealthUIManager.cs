
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesLeft;

    private void Start()
    {
        GameLoopManager.Instance.OnStateChanged += GameLoopManager_OnStateChanged;
        GameLoopManager.Instance.OnLivesLeftChanged += GameLoopManager_LivesLeftChanged;
        Hide();
    }

    private void GameLoopManager_LivesLeftChanged(object sender, System.EventArgs e)
    {
        livesLeft.text = GameLoopManager.Instance.GetLivesLeft().ToString();
    }

    private void GameLoopManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameLoopManager.Instance.IsGameActive() || GameLoopManager.Instance.IsRespawn())
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
