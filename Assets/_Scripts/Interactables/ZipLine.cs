using System;
using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Cinemachine.Utility;
using UnityEngine.Splines.Interpolators;
public class ZipLine : BaseInteractable
{
    [SerializeField] private LevelEntity startingLevel;
    [SerializeField] private LevelEntity endingLevel;
    [SerializeField] private ZiplineStartingPole startingPole;
    [SerializeField] private GameObject endingPole;
    [SerializeField] private Vector3 startingPoint;
    [SerializeField] private Vector3 endingPoint;
    [SerializeField] private float zipSpeedIncrease = 1f;
    [SerializeField] private float zipProgress = 0;
    [SerializeField] private float jumpDistanceToEnd = .5f;
    [SerializeField] private Player playerToZip;
    [SerializeField] private LineRenderer lineRenderer;
    private float startTime;
    // Time to swing trough the zipline, in seconds.
    public float journeyTime = 1.5f;


    public override void Interact(Player player)
    {
        startTime = Time.time;
        playerToZip = player;
        player.LockMovement();
        zipProgress = 0;
    }

    public void SetupZipline(LevelEntity startingLevel, LevelEntity endingLevel, Vector3 startingPoint, Vector3 endingPoint)
    {
        this.startingLevel = startingLevel;
        this.endingLevel = endingLevel;
        this.startingPoint = startingPoint;
        this.endingPoint = endingPoint;

        startingPole.transform.position = startingPoint;
        endingPole.transform.position = endingPoint;

        lineRenderer.positionCount = 100;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        /* TODO: this is the tricky part; the rendered line looks often uncanny.
        Blocker for the Zipline-feature.
        */
        Vector3 middleVector = (endingPoint - startingPoint) / 2;
        for (int i = 0; i < 100; i++)
        {
            float fracComplete = i / 100f;
            Vector3 nextPointToMiddle = Vector3.Lerp(startingPoint, middleVector, fracComplete);
            Vector3 middlePointToEnd = Vector3.Lerp(middleVector, endingPoint, fracComplete);
            Vector3 nextPoint = Vector3.Lerp(startingPoint, endingPoint, fracComplete);
            lineRenderer.SetPosition(i, nextPoint);
        }
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
        float fracComplete = (Time.time - startTime) / journeyTime;
        float3 nextPoint = Vector3.Slerp(startingPoint, endingPoint, fracComplete);
        playerToZip.gameObject.transform.position = nextPoint;
    }

    private void ZipCompleted()
    {
        playerToZip.SetLevelContainer(endingLevel);
        playerToZip.AllowMovement();
        playerToZip = null;
    }

    private void TryJumpFromZip()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        if (inputVector != Vector2.zero)
        {
            if (Vector3.Distance(playerToZip.gameObject.transform.position, endingPoint) <= jumpDistanceToEnd)
            {
                ZipCompleted();
            }
        }
    }
}
