using System;
using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Collections;

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
    [SerializeField] private bool canMove;
    private void Update()
    {

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
        gameInput.OnSelfKillAction += GameInput_OnSelfKillAction;
        animator = GetComponentInChildren<Animator>();
        isAlive = true;
        canMove = true;
    }

    private void HandleInteractions()
    {
        if (!LevelManager.Instance.IsGameActive()) return;
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


    private void GameInput_OnSelfKillAction(object sender, EventArgs e)
    {
        // For debugging only
        // if (!LevelManager.Instance.IsGameActive()) return;
        // Killed();
    }

    private void HandleMoving()
    {
        if (!canMove)
        {
            animator.SetBool("IsWalking", false);
            return;
        }
        if (!LevelManager.Instance.IsGameActive()) return;
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
        transform.position = BaseSplineMovement.GetNextPositionOnSpline(splineContainer, transform.position, inputVector.x * speed);

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
            SetSelectedInteractable(null);
            isAlive = false;
            Hide();
            // spawn PUFF element
            LevelManager.Instance.PlayerKilled();
        }
    }

    public void Revive(Transform location, SplineContainer newSpline)
    {
        isAlive = true;
        transform.position = location.position;
        SetSplineContainer(newSpline);
        Show();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void AllowMovement()
    {
        canMove = true;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    public bool IsAlive()
    {
        return isAlive;
    }
}
