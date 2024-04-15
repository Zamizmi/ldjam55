using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSetupSO", menuName = "LevelSetupSO", order = 0)]
public class LevelSetupSO : ScriptableObject
{
    [SerializeField] public BuildingLocationSO[] pinkObjectsToSpawn;
    [SerializeField] public EnemyLocationSO[] enemyObjectsToSpawn;
}
