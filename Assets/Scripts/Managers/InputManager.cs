using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, PlayerControls.IPlayerActions
{
    private PlayerControls playerControls;

    #region INPUT EVENTS

    /// <summary>
    /// Event to be raised when the player presses the jump input
    /// </summary>
    public static System.Action OnJumpPressed;

    /// <summary>
    /// Event to be raised when the player releases the jump input
    /// </summary>
    public static System.Action OnJumpReleased;

    /// <summary>
    /// Event to be raised to detect if the player has pressed or released the select key
    /// </summary>
    public static System.Action<bool> OnSelectToggled;

    /// <summary>
    /// Event to be raised to detect if the player has pressed the select key, used by the command menu
    /// </summary>
    public static System.Action OnSelectClicked;

    /// <summary>
    /// Event raised when the player presses the command key
    /// </summary>
    public static System.Action OnCommandPressed;


    #endregion

    #region INPUT DRIVEN VARIABLES

    /// <summary>
    /// The movement input vector
    /// </summary>
    public static Vector2 movementInput { get; private set; }

    #endregion

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls?.Player.SetCallbacks(this);
        playerControls?.Enable();
    }

    private void OnEnable()
    {
        playerControls?.Enable();
    }

    private void OnDisable()
    {
        playerControls?.Disable();
    }

    /// <summary>
    /// Inputs for the player movement stored in a Vector2 called movementInput; You really don't need the y values since platformer
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Inputs for the player jump
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        // TODO: Jumping and possible grounded check
        if (context.performed)
        {
            OnJumpPressed?.Invoke();
        }
        else if (context.canceled)
        {
            OnJumpReleased?.Invoke();
        }
    }

    /// <summary>
    /// Inputs for the Select Button; Left Click (select units, hold to select mult units)
    /// </summary>
    /// <param name="context"></param>
    public void OnSelect(InputAction.CallbackContext context)
    {
        // TODO: Select Units
        if (context.performed)
        {
            OnSelectToggled?.Invoke(true);
            OnSelectClicked?.Invoke();
        }
        else if (context.canceled) OnSelectToggled?.Invoke(false);
    }

    /// <summary>
    /// Inputs for Command Button; Right Click (command units to move, command units to perform an action)
    /// </summary>
    /// <param name="context"></param>
    public void OnCommand(InputAction.CallbackContext context)
    {
        // TODO: Command Units
        if (context.performed) OnCommandPressed?.Invoke();
    }
}
