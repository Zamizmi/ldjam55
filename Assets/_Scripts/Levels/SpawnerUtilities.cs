using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public static class SpawnerUtilities
{
    public static float3 GetValidPositionToSpawnForInteractable(float minDistanceBetweenBeacons, LevelEntity levelCreator, BaseInteractable[] objectsToLookOutFor)
    {
        bool recursion = false;
        float positionSpawn = UnityEngine.Random.Range(0.04f, 0.96f);
        levelCreator.GetNativeSpline().Evaluate(positionSpawn, out float3 position, out float3 tangent, out float3 upVector);
        foreach (BaseInteractable interactable in objectsToLookOutFor)
        {
            if (Vector3.Distance(interactable.transform.position, position) < minDistanceBetweenBeacons)
            {
                recursion = true;
                break;
            }
        }
        if (recursion)
        {
            return GetValidPositionToSpawnForInteractable(minDistanceBetweenBeacons, levelCreator, objectsToLookOutFor);
        }
        return position;
    }

    public static float3 GetRandomPositionOnLevel(LevelEntity levelCreator)
    {
        float positionSpawn = UnityEngine.Random.Range(0.02f, 0.98f);
        levelCreator.GetNativeSpline().Evaluate(positionSpawn, out float3 position, out float3 _tangent, out float3 _upVector);
        return position;
    }

    public static Color GetRandomColor()
    {
        float minHue = 0f;
        float maxHue = 1f;
        return UnityEngine.Random.ColorHSV(minHue, maxHue);
    }

    public static Color GetAdjustedColorForLevel(Color mainColor, int levelIndex)
    {
        Color.RGBToHSV(mainColor, out float H, out float S, out float V);
        return Color.HSVToRGB((H + 0.04f * levelIndex), GetColorSaturationForLevel(levelIndex), GetColorValueForLevel(levelIndex));
    }

    private static float GetColorValueForLevel(int levelIndex)
    {
        return levelIndex switch
        {
            0 => 0.9f,
            1 => 0.7f,
            2 => 0.6f,
            3 => 0.4f,
            _ => 0.9f,
        };
    }

    private static float GetColorSaturationForLevel(int levelIndex)
    {
        return levelIndex switch
        {
            0 => 0.30f,
            1 => 0.40f,
            2 => 0.50f,
            3 => 0.4f,
            _ => 0.10f,
        };
    }
}
