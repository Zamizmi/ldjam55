using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SummonHorsesManager : MonoBehaviour
{
    [SerializeField] private GameObject horseToSpawn;

    private void Start()
    {
        LevelContentManager.Instance.OnSummonHorses += GameLoopManager_OnSummonHorses;
    }

    private void GameLoopManager_OnSummonHorses(object sender, System.EventArgs e)
    {
        SummonHorses();
    }

    private void SummonHorses()
    {
        LevelEntity[] allLevels = FindObjectsOfType<LevelEntity>().Where((level) => level.IsReadyForUsage()).ToArray();
        foreach (LevelEntity levelEntity in allLevels)
        {
            GameObject horseObject = Instantiate(horseToSpawn, levelEntity.GetNativeSpline().EvaluatePosition(0f), Quaternion.identity);
            HorseRider horseRider = horseObject.GetComponent<HorseRider>();
            horseRider.SetupHorse(levelEntity);
        }
    }

}
