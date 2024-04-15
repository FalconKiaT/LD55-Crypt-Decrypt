using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommandMenuManager : MonoBehaviour
{
    // Boolean that will go true once the player has made an input
    public bool hasPlayerPicked { get; private set; }
    public bool isCommandMenuRendered { get; private set; }

    // Booleans that will check the input of the player
    private bool isAwaitingInput = false;
    //private bool recievedInput = false; FIXME: Necessary?
    private bool playerSelectClicked = false;
    private bool playerCommandClicked = false;

    // Bool that will return true upon the full execution of one of the buttons
    private bool buttonWasPressed;
    
    // Will hold the command type selected upon interaction with the menu 
    public CommandType selectedCommand { get; private set; }

    [Header("SETTINGS")]
    [SerializeField] private RectTransform commandMenu;
    [SerializeField] private RectTransform backgroundImage;
    [SerializeField] private float optionSpacing;

    [Header("MENU OPTIONS")]
    [SerializeField] private RectTransform nullOption;
    [SerializeField] private RectTransform moveToOption;
    [SerializeField] private RectTransform carryToOption;
    [SerializeField] private RectTransform releaseOption;
    [SerializeField] private RectTransform stackOption;
    [SerializeField] private RectTransform boardOption;
    [SerializeField] private RectTransform unloadOption;
    [SerializeField] private RectTransform fireOption;

    // Local variables
    private RectTransform startingPos;
    private Dictionary<CommandType, RectTransform> menuOptions = new();


    private void Awake()
    {
        // Variables
        startingPos = Instantiate(nullOption); // Copy

        // Populate dictionary
        menuOptions.Add(CommandType.MoveTo, moveToOption);
        menuOptions.Add(CommandType.Grab, carryToOption);
        menuOptions.Add(CommandType.Release, releaseOption);
        menuOptions.Add(CommandType.Stack, stackOption);
        menuOptions.Add(CommandType.BoardCatapult, boardOption);
        menuOptions.Add(CommandType.UnloadCatapult, unloadOption);
        menuOptions.Add(CommandType.FireCatapult, fireOption);

        // Subscribe to input
        InputManager.OnSelectClicked += OnSelectClicked;
        InputManager.OnCommandClicked += OnCommandClicked;
    }

    private void OnDestroy()
    {
        // Unsubscribe from input
        InputManager.OnSelectClicked -= OnSelectClicked;
        InputManager.OnCommandClicked -= OnCommandClicked;
    }

    /// <summary>
    /// Function called by the Unit Manager to show the command menu
    /// </summary>
    public void StartCommandMenu()
    {
        hasPlayerPicked = false;
        buttonWasPressed = false;
        selectedCommand = CommandType.NULL;

        // Reset Input checkers
        ResetClickedListenerBools();

        // Start waiting for player to make a choice
        StartCoroutine(MenuSelectionRoutine());
    }

    private IEnumerator MenuSelectionRoutine()
    {
        // Render menu
        RenderCommandMenu();

        // Set the renderer bool after finishing the command menu
        isCommandMenuRendered = true;

        // Start listening to input
        isAwaitingInput = true;

        // HACK: DEBUGGING!
        float commandMenuStartX;
        float commandMenuStartY;
        float commandMenuWidth;
        float commandMenuHeight;

        Vector2 mousePos;
        bool xRangeGood;
        bool yRangeGood;
        // Wait until the player either clicks a choice or clicks away
        while (!buttonWasPressed)
        {
            // Get current mouse position
            mousePos = Input.mousePosition;

            // Check for select clicking out of bounds of the box
            // HACK: DEBUGGING!
            commandMenuStartX = commandMenu.anchoredPosition.x;
            commandMenuStartY = commandMenu.anchoredPosition.y;
            commandMenuWidth = commandMenu.rect.width;
            commandMenuHeight = commandMenu.rect.height;

            // Check ranges
            xRangeGood = commandMenuStartX < mousePos.x && mousePos.x < commandMenuStartX + commandMenuWidth;
            yRangeGood = commandMenuStartY > mousePos.y && commandMenuStartY - commandMenuHeight < mousePos.y;

            if (xRangeGood && yRangeGood && playerSelectClicked)
            {
                // Player clicked inside the menu but not an option, ignore
                string debug = "PLAYER CLICKED INSIDE THE MENU\n";
                if (UnitManager.instance.doDebugLog) Debug.Log(debug);
                ResetClickedListenerBools();
                yield return null;
                continue;
            }
            else if ( (!xRangeGood || !yRangeGood) && playerSelectClicked)
            {
                // The player clicked away of the menu, close it and report back to the unit manager
                // Player clicked inside the menu but not an option, ignore
                string debug = "EXITED THE COMMAND MENU DUE TO CLICKING OUTSIDE\n";
                if (UnitManager.instance.doDebugLog) Debug.Log(debug);
                StartCoroutine(CloseCommandMenu());
                yield break;
            }
            // If the player has command clicked again, re-set the command menu to the mouse position
            if (playerCommandClicked)
            {
                // Re-update mouse vector positions
                UnitManager.instance.mousePosAtCommand = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                UnitManager.instance.mouseWorldPosAtCommand = Camera.main.ScreenToWorldPoint(UnitManager.instance.mousePosAtCommand);
                // Set the command menu at the new position
                commandMenu.anchoredPosition = UnitManager.instance.mousePosAtCommand;
                // Re-evaluate Command options
                UnitManager.instance.FindCommonCommands();
                UnitManager.instance.ValidateCommandsToSend(); 
                // Render menu again
                RenderCommandMenu(); // Render menu again
                // Start checking for input again
                ResetClickedListenerBools();
                // Wait a frame and continue
                yield return null;
                continue;
            }
            // Else nothing happened, wait another frame
            yield return null;
        }
        // The player made a decision by clicking a button, Stop rendering menu
        StartCoroutine(CloseCommandMenu());
    }

    private void RenderCommandMenu()
    {
        // Hide previous buttons
        UnactivateAllOptions();

        // Render the command menu at the mouse position
        commandMenu.anchoredPosition = new Vector2(UnitManager.instance.mousePosAtCommand.x, UnitManager.instance.mousePosAtCommand.y);
        commandMenu.gameObject.SetActive(true);

        // Mutating Vector2 to set offset on menu items
        Vector2 posInCommandMenu = new Vector2(startingPos.anchoredPosition.x, startingPos.anchoredPosition.y);

        // Render all valid options into the command menu
        foreach (CommandType currCommand in UnitManager.instance.validCommandsOnSelected)
        {
            if (menuOptions.TryGetValue(currCommand, out RectTransform rct))
            {
                rct.anchoredPosition = posInCommandMenu;
                posInCommandMenu = new Vector2(posInCommandMenu.x, posInCommandMenu.y - optionSpacing);
                rct.gameObject.SetActive(true);
            }
        }
        // FIXME: CHANGE BACKGROUND HEIGHT
        // FIXME: HANDLE NO OPTIONS TOO
    }

    /// <summary>
    /// Function to reset the command menu after decision was made
    /// </summary>
    private IEnumerator CloseCommandMenu()
    {
        // FIXME: REDO THESE
        UnactivateAllOptions();
        commandMenu.gameObject.SetActive(false);
        ResetClickedListenerBools();
        isCommandMenuRendered = false;
        buttonWasPressed = false;
        isAwaitingInput = false;
        // Player made a decision
        hasPlayerPicked = true;
        yield break;
    }

    /// <summary>
    /// Will execute the command selected in the menu
    /// </summary>
    public void ExecuteCommand(CommandOption input)
    {
        selectedCommand = input.getCommand();
        if (UnitManager.instance.doDebugLog) Debug.Log("SELECTED COMMAND BUTTON PRESSED = " + selectedCommand.ToString());
        // Player made a choice
        buttonWasPressed = true;
    }

    private void UnactivateAllOptions()
    {
        nullOption.gameObject.SetActive(false);
        moveToOption.gameObject.SetActive(false);
        carryToOption.gameObject.SetActive(false);
        releaseOption.gameObject.SetActive(false);
        stackOption.gameObject.SetActive(false);
        boardOption.gameObject.SetActive(false);
        unloadOption.gameObject.SetActive(false);
        fireOption.gameObject.SetActive(false); 
    }

    /// <summary>
    /// Helper function that will set the clicked bool to true after the menu was rendered
    /// </summary>
    private void OnSelectClicked()
    {
        // Only listen to the input if we are listening to input
        if (isAwaitingInput)
        {
            playerSelectClicked = true;
        }
    }

    /// <summary>
    /// Helper function that will set the command click bool to true after the menu was rendered
    /// </summary>
    private void OnCommandClicked()
    {
        // Only listen to the input if the command menu is rendered
        if (isCommandMenuRendered) playerCommandClicked = true;
    }

    /// <summary>
    /// Helper function to reset the clicker listeners
    /// </summary>
    private void ResetClickedListenerBools()
    {
        //recievedInput = false; FIXME: Necessary
        playerCommandClicked = false;
        playerSelectClicked = false;
    }
}
