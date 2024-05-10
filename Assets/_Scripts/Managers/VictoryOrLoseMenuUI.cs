using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryOrLoseMenuUI : MonoBehaviour
{

    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject wonMode;
    [SerializeField] private GameObject loseMode;

    private void Start()
    {
        GameLoopManager.Instance.OnStateChanged += LevelManager_OnStateChanged;
        Hide();
        GameInput.Instance.OnResetAction += GameInput_OnRestart;
    }

    private void GameInput_OnRestart(object sender, EventArgs e)
    {
        RestartGame();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LevelManager_OnStateChanged(object sender, System.EventArgs e)
    {
        Show();
        if (GameLoopManager.Instance.IsGameOver())
        {
            loseMode.SetActive(true);
            return;
        }
        if (GameLoopManager.Instance.IsGameWon())
        {
            wonMode.SetActive(true);
            return;
        }
        Hide();
    }

    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            RestartGame();
            Hide();
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        Time.timeScale = 1f;
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
