using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Splines;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private float eyeSight;
    [SerializeField] private SplineContainer splineContainer;
    private Animator animator;
    // false = left
    // true = right
    [SerializeField] private bool directionToGo;
    private SpriteRenderer visualRender;
    [SerializeField] private bool isMoving = true;
    [SerializeField] private float speed;
    [SerializeField] private float timeToChangeDecision = 1f;
    [SerializeField] private float decisionTimer = 0f;
    [SerializeField] private float noticeTimer = 0f;
    [SerializeField] private float noticeTime = 0.1f;
    [SerializeField] private bool canSeePlayer;
    [SerializeField] private float timeToKillPlayer = 0.2f;
    [SerializeField] private float timeDamagedPlayer;


    private void Update()
    {
        DecideDirection();
        HandleMovement();
        HandleTimers();
        HandlePlayerFollowability();
    }

    private void HandlePlayerFollowability()
    {
        float distanceToPlayer = (Player.Instance.gameObject.transform.position - transform.position).magnitude;
        bool nowCanSeePlayer = Player.Instance.GetSplineContainer() == splineContainer && distanceToPlayer < eyeSight;
        if (nowCanSeePlayer != canSeePlayer && canSeePlayer)
        {
            noticeTimer = 0f;
            animator.SetTrigger("Notice");
            isMoving = false;
        }
        canSeePlayer = nowCanSeePlayer;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == Player.Instance.gameObject)
        {
            timeDamagedPlayer += Time.deltaTime;
            if (timeDamagedPlayer > timeToKillPlayer)
            {
                Player.Instance.Killed();
            }
        }
        else
        {
            timeToKillPlayer = 0f;
        }
    }

    private void HandleTimers()
    {
        decisionTimer += Time.deltaTime;
        if (canSeePlayer)
        {
            noticeTimer += Time.deltaTime;
        }
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        visualRender = GetComponentInChildren<SpriteRenderer>();
    }

    private void DecideDirection()
    {
        if (canSeePlayer && noticeTimer > noticeTime)
        {
            directionToGo = Player.Instance.transform.position.x > transform.position.x;
            isMoving = true;
            return;
        }

        if (decisionTimer > timeToChangeDecision)
        {
            // 0, 1, 2
            int decision = UnityEngine.Random.Range(0, 3);
            decisionTimer = 0f;
            if (decision == 2)
            {
                isMoving = false;
                return;
            }
            isMoving = true;
            if (decision == 1)
            {
                directionToGo = true;
            }
            else
            {
                directionToGo = false;
            }
        }
    }

    private void HandleMovement()
    {
        animator.SetBool("IsWalking", isMoving);
        if (!isMoving) return;
        if (directionToGo)
        {
            visualRender.flipX = false;
        }
        else
        {
            visualRender.flipX = true;
        }
        float directionX = directionToGo ? 1 : -1;
        float4 nearestAfterMovingX = new float4(directionX, 0, 0, float.PositiveInfinity);
        NativeSpline native = new NativeSpline(splineContainer.Spline, splineContainer.transform.localToWorldMatrix);
        float d = SplineUtility.GetNearestPoint(native, transform.position + new Vector3(directionX * speed * Time.deltaTime, 0f, 0f), out float3 p, out float t);
        if (d < nearestAfterMovingX.w)
            nearestAfterMovingX = new float4(p, d);
        transform.position = nearestAfterMovingX.xyz;
    }

    public void SetSplineContainer(SplineContainer newSpline)
    {
        splineContainer = newSpline;
    }
}
