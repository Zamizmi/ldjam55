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
        LevelManager.Instance.OnStateChanged += LevelManager_OnStateChanged;
        Hide();
    }

    private void LevelManager_OnStateChanged(object sender, System.EventArgs e)
    {
        Show();
        if (LevelManager.Instance.IsGameOver())
        {
            loseMode.SetActive(true);
            return;
        }
        if (LevelManager.Instance.IsGameWon())
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
