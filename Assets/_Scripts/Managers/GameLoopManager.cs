using System;
using System.Linq;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager Instance
    {
        get;
        private set;
    }
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State changedState;
    }
    public event EventHandler OnBeaconCountChanged;

    public event EventHandler OnLivesLeftChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    public enum State
    {
        WaitingToStart,
        MoonPhaseAnimation,
        Respawning,
        GamePlaying,
        GameOver,
        GameWon,
        LevelWon,
    }

    [SerializeField] private State state;
    private bool isGamePaused = false;

    [SerializeField] private int beaconsActive;
    [SerializeField] private int beaconsMax;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel;
    [SerializeField] private int livesLeft = 3;
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private LevelContentManager levelContentManager;
    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        maxLevel = levelContentManager.GetMaxLevel();
    }

    public void StartGame()
    {
        levelContentManager.OnBeaconSpawned += LevelContentManager_OnStateChanged;
        TogglePauseGame(false);
        OnLivesLeftChanged?.Invoke(this, EventArgs.Empty);
        OnBeaconCountChanged?.Invoke(this, EventArgs.Empty);
        state = State.GamePlaying;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            changedState = state,
        });
        UpdateBeaconMax();
    }

    private void LevelContentManager_OnStateChanged(object sender, System.EventArgs e)
    {
        UpdateBeaconMax();
    }

    private void UpdateBeaconMax()
    {
        beaconsMax = levelContentManager.GetBeaconCount();
        state = State.GamePlaying;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            changedState = state,
        });
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                // we just wait for the animation to be over
                break;
            case State.Respawning:
                break;
            case State.LevelWon:
                currentLevel++;
                // Play animations!
                if (currentLevel > maxLevel - 1)
                {
                    state = State.GameWon;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        changedState = state,
                    });
                }
                else
                {
                    UpdateBeaconMax();
                }
                break;
            case State.GamePlaying:
                if (beaconsActive >= beaconsMax && beaconsMax > 0)
                {
                    Debug.Log("Trying to win!");
                    state = State.LevelWon;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        changedState = state,
                    });
                }
                break;
            case State.GameWon:
                TogglePauseGame(false);
                break;
            case State.GameOver:
                TogglePauseGame(false);
                break;
        }
    }
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public bool IsGameWon()
    {
        return state == State.GameWon;
    }

    public bool IsGameActive()
    {
        return state == State.GamePlaying;
    }

    public void IncreaseActiveBeaconCount()
    {
        beaconsActive++;
        if (beaconsActive % 3 == 0)
        {
            livesLeft++;
            OnLivesLeftChanged?.Invoke(this, EventArgs.Empty);
        }
        OnBeaconCountChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetActiveBeaconCount()
    {
        return beaconsActive;
    }

    public int GetLivesLeft()
    {
        return livesLeft;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame(!isGamePaused);
    }

    public bool IsRespawn()
    {
        return state == State.Respawning;
    }

    private void TogglePauseGame(bool newState)
    {
        isGamePaused = newState;
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

    public void PlayerKilled()
    {
        if (state == State.GamePlaying)
        {

            livesLeft--;
            OnLivesLeftChanged?.Invoke(this, EventArgs.Empty);
            if (livesLeft > 0)
            {
                state = State.Respawning;
            }
            else
            {
                state = State.GameOver;
            }
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                changedState = state,
            });
        }
    }

    public void SpawnPlace(LevelEntity location, Transform transform)
    {
        // Player.Instance.Revive(transform, GetSplineByName(location));
        state = State.GamePlaying;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            changedState = state,
        });
    }
}
