using System;
using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Cinemachine.Utility;
public class ZipLine : BaseInteractable
{
    [SerializeField] private SplineContainer startingSpline;
    [SerializeField] private SplineContainer endingSpline;
    [SerializeField] private SplineContainer zipSpline;
    [SerializeField] private float maxZipSpeed = 5f;
    [SerializeField] private float zipSpeedIncrease = 0.01f;
    [SerializeField] private float zipProgress = 0;
    [SerializeField] private float jumpDistanceToEnd = .5f;
    [SerializeField] private Player playerToZip;


    public override void Interact(Player player)
    {
        playerToZip = player;
        player.LockMovement();
    }

    private void Update()
    {
        if (playerToZip != null)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        if (zipProgress < 1f)
        {
            zipProgress += zipSpeedIncrease;
        }
        float3 nextPoint = zipSpline.Spline.EvaluatePosition(zipProgress);
        float3 endPoint = zipSpline.Spline.EvaluatePosition(1f);
        playerToZip.gameObject.transform.position = nextPoint;
        if (Vector3.Distance(playerToZip.gameObject.transform.position, endPoint) <= jumpDistanceToEnd)
        {
            ZipCompleted();
        }
    }

    private void ZipCompleted()
    {
        playerToZip.SetSplineContainer(endingSpline);
        playerToZip.AllowMovement();
        playerToZip = null;
    }
}
