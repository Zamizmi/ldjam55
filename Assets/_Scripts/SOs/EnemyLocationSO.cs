using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLocationSO", menuName = "EnemyLocationSO", order = 0)]
public class EnemyLocationSO : ScriptableObject
{
    [SerializeField] public LevelManager.LevelColor levelName;
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public Vector3 locationVector;
}
