using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private float eyeSight;
    [SerializeField] private LevelEntity currentLevel;
    protected Animator animator;
    // false = left
    // true = right
    [SerializeField] protected bool directionToGo;
    protected SpriteRenderer visualRender;
    [SerializeField] private bool isMoving = true;
    [SerializeField] private float speed;
    [SerializeField] private float decisionTimer = 0f;
    [SerializeField] private float noticeTimer = 0f;
    [SerializeField] private float noticeTime = 0.1f;
    [SerializeField] private bool canSeePlayer;
    [SerializeField] private float timeToKillPlayer = 0.2f;
    [SerializeField] private float timeDamagedPlayer;
    [SerializeField] private GameObject statusIndicator;


    private void Update()
    {
        HandleMovement();
        HandleTimers();
        HandlePlayerFollowability();
    }

    private void HandlePlayerFollowability()
    {
        float distanceToPlayer = (Player.Instance.gameObject.transform.position - transform.position).magnitude;
        bool nowCanSeePlayer = Player.Instance.GetLevel() == currentLevel && distanceToPlayer < eyeSight && Player.Instance.IsAlive();
        if (nowCanSeePlayer != canSeePlayer && canSeePlayer)
        {
            noticeTimer = 0f;
            animator.SetTrigger("Notice");
            isMoving = false;
        }
        canSeePlayer = nowCanSeePlayer;
        statusIndicator.SetActive(canSeePlayer);
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
        transform.position = BaseSplineMovement.GetNextPositionOnSpline(currentLevel.GetNativeSpline(), transform.position, directionX * speed);
    }

    public void SetLevelContainer(LevelEntity newLevel)
    {
        currentLevel = newLevel;
    }
}
