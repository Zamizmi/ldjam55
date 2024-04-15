using System;
using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Player : MonoBehaviour
{

    public static Player Instance
    {
        get;
        private set;
    }

    public event EventHandler<OnSelectedInteractableChangedEventArgs> OnSelectedInteractableChanged;


    public class OnSelectedInteractableChangedEventArgs : EventArgs
    {
        public BaseInteractable selectedInteractable;
    }

    [SerializeField] private float speed;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private SplineContainer splineContainer;
    private float interactionDistance = 1.5f;
    [SerializeField] private SpriteRenderer visualRender;
    [SerializeField] private BaseInteractable selectedInteractable;
    private Vector2 lastInteractionVector;
    private Animator animator;
    private bool isAlive;
    private void Update()
    {
        if (!LevelManager.Instance.IsGameActive()) return;
        HandleMoving();
        HandleInteractions();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player!");
        }
        Instance = this;
    }


    private void Start()
    {
        gameInput.OnInteractHandler += GameInput_OnInteractHandler;
        animator = GetComponentInChildren<Animator>();
        isAlive = true;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        if (inputVector != Vector2.zero)
        {
            lastInteractionVector = new Vector2(inputVector.x, 0f);
        }

        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, lastInteractionVector, interactionDistance, interactableLayerMask);
        if (raycastHit)
        {
            if (raycastHit.transform.TryGetComponent(out BaseInteractable interactable))
            {
                if (selectedInteractable != interactable)
                {
                    SetSelectedInteractable(interactable);
                }
            }
            else
            {
                SetSelectedInteractable(null);
            }
        }
        else
        {
            SetSelectedInteractable(null);
        }

    }

    private void HandleMoving()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        if (inputVector.x == 0)
        {
            animator.SetBool("IsWalking", false);
            return;
        }
        animator.SetBool("IsWalking", true);
        if (lastInteractionVector.x > 0)
        {
            visualRender.flipX = false;
        }
        else
        {
            visualRender.flipX = true;
        }
        float4 nearestAfterMovingX = new float4(inputVector.x, 0, 0, float.PositiveInfinity);
        NativeSpline native = new NativeSpline(splineContainer.Spline, splineContainer.transform.localToWorldMatrix);
        float d = SplineUtility.GetNearestPoint(native, transform.position + new Vector3(inputVector.x * speed * Time.deltaTime, 0f, 0f), out float3 p, out float t);
        if (d < nearestAfterMovingX.w)
            nearestAfterMovingX = new float4(p, d);
        transform.position = nearestAfterMovingX.xyz;
    }

    private void GameInput_OnInteractHandler(object sender, System.EventArgs e)
    {
        if (selectedInteractable != null)
        {
            selectedInteractable?.Interact(this);
        }
    }

    private void SetSelectedInteractable(BaseInteractable interactable)
    {
        this.selectedInteractable = interactable;
        OnSelectedInteractableChanged?.Invoke(this, new OnSelectedInteractableChangedEventArgs
        {
            selectedInteractable = interactable
        });
    }

    public void SetSplineContainer(SplineContainer newSpline)
    {
        splineContainer = newSpline;
    }

    public void SetSpriteLevel(int newSpriteLevel)
    {
        visualRender.sortingOrder = newSpriteLevel;
    }

    public SplineContainer GetSplineContainer()
    {
        return splineContainer;
    }

    public void Killed()
    {
        if (isAlive)
        {
            LevelManager.Instance.PlayerKilled();
        }
    }

    public void Revive(Transform location, SplineContainer newSpline)
    {
        isAlive = true;
        transform.position = location.position;
        SetSplineContainer(newSpline);
    }
}
