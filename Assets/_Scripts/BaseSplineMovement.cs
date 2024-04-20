using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public static class BaseSplineMovement
{
    public static float3 GetNextPositionOnSpline(SplineContainer splineContainer, Vector3 currentPosition, float xValueToMoveInto)
    {
        float3 endPoint = splineContainer.Spline.EvaluatePosition(1f);
        float3 startPoint = splineContainer.Spline.EvaluatePosition(0f);
        NativeSpline native = new NativeSpline(splineContainer.Spline, splineContainer.transform.localToWorldMatrix);
        SplineUtility.GetNearestPoint(native, currentPosition + new Vector3(xValueToMoveInto * Time.deltaTime, 0f, 0f), out float3 nextPosition, out float t);
        float distanceToStart = Vector3.Distance(nextPosition, startPoint);
        float distanceToEnd = Vector3.Distance(nextPosition, endPoint);
        if (distanceToEnd <= .1f && xValueToMoveInto > 0)
        {
            return splineContainer.Spline.EvaluatePosition(0f);
        }
        if (distanceToStart <= .1f && xValueToMoveInto < 0)
        {
            return splineContainer.Spline.EvaluatePosition(1f);
        }

        return nextPosition.xyz;
    }
}
