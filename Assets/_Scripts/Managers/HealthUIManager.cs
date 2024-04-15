
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesLeft;

    private void Start()
    {
        LevelManager.Instance.OnStateChanged += LevelManager_OnStateChanged;
        Hide();
    }

    private void LevelManager_LivesLeftChanged(object sender, System.EventArgs e)
    {
        livesLeft.text = LevelManager.Instance.GetLivesLeft().ToString();
    }

    private void LevelManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (LevelManager.Instance.IsGameActive())
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
