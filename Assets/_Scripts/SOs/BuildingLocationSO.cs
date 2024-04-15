using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingLocationSO", menuName = "BuildingLocationSO", order = 0)]
public class BuildingLocationSO : ScriptableObject
{
    [SerializeField] public string levelName;
    [SerializeField] public GameObject buildingPrefab;
    [SerializeField] public Vector3 locationVector;
}
