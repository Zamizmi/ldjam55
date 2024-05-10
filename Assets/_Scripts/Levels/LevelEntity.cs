using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;

public class LevelEntity : MonoBehaviour
{
    [SerializeField]
    private SpriteShapeController spriteShapeController;
    private SpriteShapeRenderer spriteShapeRenderer;
    private UnityEngine.Splines.Spline nativeSpline;
    [SerializeField] private List<float3> nodeList = new List<float3>();
    [Range(4, 100)]
    [SerializeField] private GameObject beaconToSpawn;
    [SerializeField] private GameObject teleportToSpawn;
    [SerializeField] private int heightIndex;
    private bool readyForUsage = false;
    private int heightOfLevel = 4;
    private void Awake()
    {
        spriteShapeController = GetComponent<SpriteShapeController>();
        spriteShapeRenderer = GetComponent<SpriteShapeRenderer>();
    }

    public void SetupWithoutSpline(int pointCount, float xMultiplier, int heightIndex, int sortingLayerId, Color mainColor)
    {
        this.heightIndex = heightIndex;

        AddRandomPointsToSpline(pointCount, xMultiplier, heightIndex);
        SetupVisualSpline();
        spriteShapeRenderer.sortingLayerID = sortingLayerId;
        spriteShapeRenderer.color = SpawnerUtilities.GetAdjustedColorForLevel(mainColor, heightIndex);
    }

    public void LevelReadyForUsage()
    {
        readyForUsage = true;
    }

    public bool IsReadyForUsage()
    {
        return readyForUsage;
    }

    public Teleporter SpawnTeleportToIndex(float3 position, Color mainColor)
    {
        GameObject teleportObject = Instantiate(teleportToSpawn, position, Quaternion.identity, this.transform);
        Teleporter teleport = teleportObject.GetComponent<Teleporter>();
        teleport.SetCurrentLevel(this);
        teleport.SetRenderColor(SpawnerUtilities.GetAdjustedColorForLevel(mainColor, heightIndex));
        return teleport;
    }
    private void AddRandomPointsToSpline(int pointCount, float xMultiplier, float yOffset)
    {
        // Used to reduce the area of the empty sky
        float heightIncreasePerLevel = 1.5f;
        float defaultIncrease = 1f;
        nodeList.Clear();
        // Set the first node to very left, so spline is in the middle.
        nodeList.Add(new Vector3(-pointCount * xMultiplier / 2, -heightOfLevel, 0));
        // Makes the levels to start from different points of the curve.
        float xOffset = UnityEngine.Random.Range(4f, 13f);
        for (int i = 0; i < pointCount; i++)
        {
            nodeList.Add(new float3(i * xMultiplier - pointCount * xMultiplier / 2, Mathf.Sin((i + xOffset) / xOffset) / 2 - heightIncreasePerLevel * (yOffset - 1) + defaultIncrease, 0));
        }
        nodeList.Add(new Vector3(nodeList.Last().x, -heightOfLevel, 0));
    }



    private void SetupVisualSpline()
    {
        UnityEngine.U2D.Spline levelSpline = spriteShapeController.spline;
        levelSpline.Clear();
        for (int i = 0; i < nodeList.Count(); i++)
        {
            levelSpline.InsertPointAt(i, nodeList[i]);
            levelSpline.SetTangentMode(i, ShapeTangentMode.Continuous);
            levelSpline.SetHeight(i, 0f);
        }
        // Set sharp corners
        levelSpline.SetTangentMode(0, ShapeTangentMode.Linear);
        levelSpline.SetTangentMode(levelSpline.GetPointCount() - 1, ShapeTangentMode.Linear);
        SetupNativeSpline();
    }

    public UnityEngine.Splines.Spline GetNativeSpline()
    {
        return nativeSpline;
    }

    private void SetupNativeSpline()
    {
        int count = spriteShapeController.spline.GetPointCount();
        nativeSpline = new UnityEngine.Splines.Spline(count - 1);
        for (int i = 1; i < count - 1; i++)
        {
            nativeSpline.Add(new BezierKnot(spriteShapeController.spline.GetPosition(i)));
        }
    }
}