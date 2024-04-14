using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance
    {
        get;
        private set;
    }
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private enum State
    {
        WaitingToStart,
        GamePlaying,
        GameOver,
        GameWon,
    }

    private State state;
    private bool isGamePaused = false;

    private float gamePlayingTimer;
    [SerializeField] private float gamePlayingTimerMax = 30f;
    [SerializeField] private int beaconsActive;
    [SerializeField] private int beaconsMax;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        gamePlayingTimer = gamePlayingTimerMax;
        Beacon[] beacons = FindObjectsOfType<Beacon>();
        beaconsMax = beacons.Length;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
            // we just wait for the animation to be over
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (beaconsActive >= beaconsMax)
                {
                    state = State.GameWon;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public bool IsGamePaused()
    {
        return state != State.GamePlaying;
    }

    public float GetPlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);

        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);

        }
    }

}
