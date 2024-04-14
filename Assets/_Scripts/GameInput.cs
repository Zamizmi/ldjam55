using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInput gameInputActions;
    public event EventHandler OnInteractHandler;
    public event EventHandler OnPauseAction;


    private void Awake()
    {
        Instance = this;
        gameInputActions = new PlayerInput();
        gameInputActions.Player.Enable();

        gameInputActions.Player.Interact.performed += Interact_performed;
        gameInputActions.Player.Pause.performed += Pause_performed;

    }

    private void OnDestroy()
    {
        gameInputActions.Player.Interact.performed -= Interact_performed;
        gameInputActions.Player.Pause.performed -= Pause_performed;

        gameInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }


    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = gameInputActions.Player.Movement.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractHandler?.Invoke(this, EventArgs.Empty);
    }
}
