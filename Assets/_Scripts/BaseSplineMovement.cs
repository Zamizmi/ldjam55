using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public static class BaseSplineMovement
{
    public static float3 GetNextPositionOnSpline(Spline native, Vector3 currentPosition, float xValueToMoveInto)
    {
        float3 endPoint = native.EvaluatePosition(1f);
        float3 startPoint = native.EvaluatePosition(0f);
        SplineUtility.GetNearestPoint(native, currentPosition + new Vector3(xValueToMoveInto * Time.deltaTime, 0f, 0f), out float3 nextPosition, out float t);
        float distanceToStart = Vector3.Distance(nextPosition, startPoint);
        float distanceToEnd = Vector3.Distance(nextPosition, endPoint);
        if (distanceToEnd <= .1f && xValueToMoveInto > 0)
        {
            return native.EvaluatePosition(0f);
        }
        if (distanceToStart <= .1f && xValueToMoveInto < 0)
        {
            return native.EvaluatePosition(1f);
        }
        return nextPosition.xyz;
    }
}
