using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance
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
    public class OnBeaconCountChangedEventArgs : EventArgs
    {
        public int beaconCount;
    }

    public event EventHandler OnLivesLeftChanged;
    public class OnLivesLeftChangedEventArgs : EventArgs
    {
        public int livesLeft;
    }
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

    public enum LevelColor
    {
        Pink,
        Purple,
        Black
    }

    [SerializeField] private State state;
    private bool isGamePaused = false;

    private float gamePlayingTimer;
    [SerializeField] private float gamePlayingTimerMax = 30f;
    [SerializeField] private int beaconsActive;
    [SerializeField] private int beaconsMax;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel;
    [SerializeField] private LevelSetupSO[] levelSetups;
    [SerializeField] private GameObject startBeaconVisual;
    [SerializeField] private int livesLeft = 3;
    [SerializeField] private SplineContainer pinkSpline;
    [SerializeField] private SplineContainer purpleSpline;
    [SerializeField] private SplineContainer blackSpline;
    [SerializeField] private GameObject horses;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        maxLevel = levelSetups.Length;
    }

    public void StartGame()
    {
        TogglePauseGame(false);
        HideStartBeacon();
        ProgressLevel();
        OnLivesLeftChanged?.Invoke(this, new OnLivesLeftChangedEventArgs
        {
            livesLeft = this.livesLeft,
        });
    }

    private void HideStartBeacon()
    {
        startBeaconVisual.SetActive(false);
    }

    // Made public so cut scenes can call this from animation events
    public void ProgressLevel()
    {
        OnBeaconCountChanged?.Invoke(this, new OnBeaconCountChangedEventArgs
        {
            beaconCount = beaconsActive
        });

        gamePlayingTimer = gamePlayingTimerMax;
        LevelSetupSO nextLevel = levelSetups[currentLevel];
        foreach (BuildingLocationSO toSpawn in nextLevel.pinkObjectsToSpawn)
        {
            Instantiate(toSpawn.buildingPrefab, toSpawn.locationVector, Quaternion.identity);
        }
        foreach (EnemyLocationSO toSpawn in nextLevel.enemyObjectsToSpawn)
        {
            GameObject created = Instantiate(toSpawn.enemyPrefab, toSpawn.locationVector, Quaternion.identity);
            BaseEnemy enemy = created.GetComponent<BaseEnemy>();
            enemy.SetSplineContainer(GetSplineByName(toSpawn.levelName));
        }
        Beacon[] beacons = FindObjectsOfType<Beacon>().Where((beacon) => beacon.gameObject.activeSelf).ToArray();
        beaconsMax = beacons.Length;
        state = State.GamePlaying;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            changedState = state,
        });
    }

    private SplineContainer GetSplineByName(LevelColor levelName)
    {
        if (levelName == LevelColor.Pink) return pinkSpline;
        if (levelName == LevelColor.Purple) return purpleSpline;
        if (levelName == LevelColor.Black) return blackSpline;
        return blackSpline;
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
                    SummonHorses();
                }
                else
                {
                    ProgressLevel();
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (beaconsActive >= beaconsMax && beaconsMax > 0)
                {
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

    private void SummonHorses()
    {
        horses.SetActive(true);
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

    public bool IsLevelWon()
    {
        return state == State.LevelWon;
    }

    public void IncreaseActiveBeaconCount()
    {
        beaconsActive++;
        OnBeaconCountChanged?.Invoke(this, new OnBeaconCountChangedEventArgs
        {
            beaconCount = beaconsActive
        });
    }

    public float GetPlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
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

    public void TogglePauseGame(bool newState)
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
            OnLivesLeftChanged?.Invoke(this, new OnLivesLeftChangedEventArgs
            {
                livesLeft = this.livesLeft,
            });
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

    public void SpawnPlace(LevelColor location, Transform transform)
    {
        Player.Instance.Revive(transform, GetSplineByName(location));
        state = State.GamePlaying;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            changedState = state,
        });
    }
}
