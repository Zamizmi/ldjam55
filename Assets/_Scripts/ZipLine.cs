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
    [SerializeField] private float zipSpeed = 5f;
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
        playerToZip.gameObject.transform.position = BaseSplineMovement.GetNextPositionOnSpline(zipSpline, playerToZip.gameObject.transform.position, playerToZip.gameObject.transform.position.x * zipSpeed);
        float3 endPoint = zipSpline.Spline.EvaluatePosition(1f);
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
