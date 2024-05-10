using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSetupSO", menuName = "LevelSetupSO", order = 0)]
public class LevelSetupSO : ScriptableObject
{
    [SerializeField] public EnemyLocationSO[] enemyObjectsToSpawn;
    [Range(1, 10)]
    [SerializeField] public int levelCount;
}
