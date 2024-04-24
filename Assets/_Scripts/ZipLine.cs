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
    [SerializeField] private float zipSpeedIncrease = 1f;
    [SerializeField] private float zipProgress = 0;
    [SerializeField] private float jumpDistanceToEnd = .5f;
    [SerializeField] private Player playerToZip;


    public override void Interact(Player player)
    {
        playerToZip = player;
        player.LockMovement();
        playerToZip.SetSplineContainer(zipSpline);
        zipProgress = 0;
    }

    private void Update()
    {
        if (playerToZip != null)
        {
            MovePlayer();
            TryJumpFromZip();
        }
    }

    private void MovePlayer()
    {
        if (zipProgress < 1f)
        {
            zipProgress += zipSpeedIncrease * Time.deltaTime;
        }
        float3 nextPoint = zipSpline.Spline.EvaluatePosition(zipProgress);
        playerToZip.gameObject.transform.position = nextPoint;
    }

    private void ZipCompleted()
    {
        playerToZip.SetSplineContainer(endingSpline);
        playerToZip.AllowMovement();
        playerToZip = null;
    }

    private void TryJumpFromZip()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        if (inputVector != Vector2.zero)
        {
            float3 endPoint = zipSpline.Spline.EvaluatePosition(1f);
            if (Vector3.Distance(playerToZip.gameObject.transform.position, endPoint) <= jumpDistanceToEnd)
            {
                ZipCompleted();
            }
        }
    }
}
