using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class LevelContentManager : MonoBehaviour
{

    public event EventHandler OnSummonHorses;

    public static LevelContentManager Instance
    {
        get;
        private set;
    }

    public event EventHandler<EventArgs> OnBeaconSpawned;
    [SerializeField] private GameObject beaconToSpawn;
    [SerializeField] private GameObject teleportToSpawn;
    [SerializeField] private GameObject ziplineToSpawn;
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private List<LevelEntity> levels = new List<LevelEntity>();
    private int nextLevelToSetup = 0;
    [SerializeField] private LevelSetupSO[] levelSetups;
    [Range(4, 250)]
    [SerializeField] private int nodeCountPerLevel;
    [Range(0.1f, 1f)]
    [SerializeField] private float xMultiplier = .25f;
    [SerializeField] private Color levelColor;

    private void Awake()
    {
        Instance = this;
        levelColor = SpawnerUtilities.GetRandomColor();
    }

    private void Start()
    {
        GameLoopManager.Instance.OnStateChanged += LevelManager_OnStateChanged;
        SetupLevel();
    }


    private void LevelManager_OnStateChanged(object sender, GameLoopManager.OnStateChangedEventArgs e)
    {
        if (e.changedState == GameLoopManager.State.LevelWon)
        {
            ProgressLevel();
        }
    }

    public int GetMaxLevel()
    {
        return levelSetups.Length;
    }

    private LevelEntity SpawnLevel()
    {
        GameObject levelEntityObject = Instantiate(levelPrefab, transform.position, Quaternion.identity);
        levelEntityObject.name = (nextLevelToSetup + 1).ToSafeString() + "Level";
        LevelEntity spawnedLevel = levelEntityObject.GetComponent<LevelEntity>();
        levels.Add(spawnedLevel);
        spawnedLevel.SetupWithoutSpline(nodeCountPerLevel, xMultiplier, nextLevelToSetup, SortingLayer.NameToID((nextLevelToSetup + 1).ToSafeString() + "Level"), levelColor);
        return spawnedLevel;
    }

    public void LevelReady()
    {
        OnSummonHorses?.Invoke(this, EventArgs.Empty);
    }

    private void SetupLevel()
    {
        LevelEntity spawnedLevel = SpawnLevel();
        {
            Player.Instance.SetLevelContainer(spawnedLevel);
            Player.Instance.transform.position = spawnedLevel.GetNativeSpline().EvaluatePosition(0.5f);
        }
        SpawnBeacons(spawnedLevel);
    }

    private void ProgressLevel()
    {
        nextLevelToSetup++;
        if (levelSetups.Length <= nextLevelToSetup) return;
        LevelEntity lastLevel = levels.Last();
        LevelSetupSO nextLevel = levelSetups[nextLevelToSetup];
        LevelEntity spawnedLevel = SpawnLevel();
        SpawnTeleportsForLevelsAndPairThem(lastLevel, spawnedLevel);
        // Leave ziplines out for now; the visuals with splines are too difficult
        // SpawnZiplineForLevelsAndPairThem(lastLevel, spawnedLevel);
        SpawnBeacons(spawnedLevel);
        foreach (EnemyLocationSO toSpawn in nextLevel.enemyObjectsToSpawn)
        {
            SpawnEnemyOnLevel(spawnedLevel, toSpawn.enemyPrefab);
        }
    }

    private void SpawnEnemyOnLevel(LevelEntity levelEntity, GameObject enemyPrefab)
    {
        GameObject created = Instantiate(enemyPrefab, SpawnerUtilities.GetRandomPositionOnLevel(levelEntity), Quaternion.identity);
        BaseEnemy enemy = created.GetComponent<BaseEnemy>();
        enemy.SetLevelContainer(levelEntity);
    }

    public int GetBeaconCount()
    {
        return FindObjectsOfType<Beacon>().Count();
    }

    private void SpawnTeleportsForLevelsAndPairThem(LevelEntity firstLevel, LevelEntity secondLevel)
    {
        Color mainColor = SpawnerUtilities.GetRandomColor();
        Teleporter firstTeleport = firstLevel.SpawnTeleportToIndex(SpawnerUtilities.GetValidPositionToSpawnForInteractable(2f, firstLevel, FindObjectsOfType<BaseInteractable>()), mainColor);
        Teleporter secondTeleport = secondLevel.SpawnTeleportToIndex(SpawnerUtilities.GetValidPositionToSpawnForInteractable(2f, secondLevel, FindObjectsOfType<BaseInteractable>()), mainColor);
        firstTeleport.PairWithTeleport(secondTeleport);
    }

    // Ziplines are a bit more tricky to get to work as intended, so for now going without them
    private void SpawnZiplineForLevelsAndPairThem(LevelEntity firstLevel, LevelEntity secondLevel)
    {
        GameObject ziplineObject = Instantiate(ziplineToSpawn, SpawnerUtilities.GetValidPositionToSpawnForInteractable(1f, firstLevel, FindObjectsOfType<BaseInteractable>()), Quaternion.identity, this.transform);
        ZipLine zipline = ziplineObject.GetComponent<ZipLine>();
        Vector3 startingPosition = SpawnerUtilities.GetValidPositionToSpawnForInteractable(1f, firstLevel, FindObjectsOfType<BaseInteractable>());
        Vector3 endingPosition = SpawnerUtilities.GetValidPositionToSpawnForInteractable(1f, secondLevel, FindObjectsOfType<BaseInteractable>());
        zipline.SetupZipline(firstLevel, secondLevel, startingPosition, endingPosition);
    }

    private void SpawnBeacons(LevelEntity levelToAdd)
    {
        BaseInteractable[] existingInteractables = FindObjectsOfType<BaseInteractable>();
        float3 position = SpawnerUtilities.GetValidPositionToSpawnForInteractable(1.5f, levelToAdd, existingInteractables);
        Instantiate(beaconToSpawn, position, Quaternion.identity, levelToAdd.transform);
        OnBeaconSpawned?.Invoke(this, EventArgs.Empty);
    }
}
