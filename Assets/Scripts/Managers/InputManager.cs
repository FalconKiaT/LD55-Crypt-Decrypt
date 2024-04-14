using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, PlayerControls.IPlayerActions
{
    public static Vector2 movementInput {get; private set;}
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
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        //Debug.Log(movementInput);
    }

    /// <summary>
    /// Inputs for the player jump
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="System.NotImplementedException"></exception>
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
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnSelect(InputAction.CallbackContext context)
    {
        // TODO: Select Units
        
    }

    /// <summary>
    /// Inputs for Command Button; Right Click (command units to move, command units to perform an action)
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnCommand(InputAction.CallbackContext context)
    {
        // TODO: Command Units
        
    }
}
