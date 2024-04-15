using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.EventSystems.EventTrigger;

public enum CommandType
{
    MoveTo,
    Grab,
    Release,
    Stack,
    BoardCatapult,
    UnloadCatapult,
    FireCatapult,
    NULL
}

public class UnitManager : MonoBehaviour
{
    // Singleton
    public static UnitManager instance;

    // Lock bool. CAREFUL, WILL STOP THE SYSTEM
    public bool isSelectCommandSystemLocked { get; private set; }
    
    private List<Entity> selectedUnits = new();

    private List<Entity> activeUnits = new();

    [Header("SETTINGS")]
    public LayerMask validCommandLayers;

    // Accessed by CommandMenuManager
    public List<CommandType> validCommandsOnSelected { get; private set; }
    
    // Vectors will be shared with the command menu
    [HideInInspector] public Vector2 mousePosAtCommand = Vector2.zero;
    [HideInInspector] public Vector2 mouseWorldPosAtCommand = Vector2.zero;

    // Entity at mouse position on command
    public Entity entityAtCommandPos { get; private set; }

    // Command list for all different types of entities, shouldnt be modified
    public List<CommandType> allCommands { get; private set; }
    public List<CommandType> bonemanCommands { get; private set; }
    public List<CommandType> armoredCommands { get; private set; }
    public List<CommandType> catapultCommands { get; private set; }

    // Local Variables
    private Coroutine PipelineRoutine = null;
    private SelectionManager selectionManager;
    private CommandMenuManager commandMenuManager;
    private List<CommandType> commonCommands = new();
    private List<CommandType> validCommands = new();

    // Input drive variables
    private bool isAwaitingInput = false;
    private bool recievedInput = false;
    private bool hasPlayerSelectClicked = false;
    private bool hasPlayerCommandClicked = false;

    // HACK: DEBUGGING
    public readonly bool doDebugLog = false;

    private void Awake()
    {
        // Handle singleton
        if (instance != null) Destroy(gameObject);
        instance = this;

        // Get components
        selectionManager = GetComponent<SelectionManager>();
        commandMenuManager = GetComponent<CommandMenuManager>();

        // Subscribe to input events
        InputManager.OnCommandClicked += OnCommandClicked;
        InputManager.OnSelectClicked += OnSelectClicked;

        // Initialize array
        validCommandsOnSelected = new();

        #region ALL COMMANDS
        allCommands = new()
        {
            CommandType.Grab,
            CommandType.MoveTo,
            CommandType.Stack,
            CommandType.BoardCatapult,
            CommandType.UnloadCatapult,
            CommandType.FireCatapult,
            CommandType.Release
        };
        #endregion

        #region BONEMAN COMMANDS
        bonemanCommands = new()
        {
            CommandType.Grab,
            CommandType.Release,
            CommandType.MoveTo,
            CommandType.Stack,
            CommandType.BoardCatapult
        };
        #endregion

        #region ARMORED BONEMAN COMMANDS
        armoredCommands = new()
        {
            CommandType.MoveTo,
            CommandType.BoardCatapult,
        };
        #endregion

        #region CATAPULT COMMANDS
        catapultCommands = new()
        {
            CommandType.MoveTo,
            CommandType.UnloadCatapult,
            CommandType.FireCatapult
        };
        #endregion
    }

    private void OnDestroy()
    {
        // Unsubscribe from input events
        InputManager.OnCommandClicked -= OnCommandClicked;
        InputManager.OnSelectClicked -= OnSelectClicked;

        // Handle singleton
        instance = null;
    }

    private void Start()
    {
        // Start the selection-command pipeline
        if (PipelineRoutine == null)
        {
            PipelineRoutine = StartCoroutine(SelectionToCommandPipeline());
        }
    }

    /// <summary>
    /// Master routine to handle the selection to command mechanics
    /// </summary>
    private IEnumerator SelectionToCommandPipeline()
    {
        // FIXME: If the game was paused, wait to depause?
        ResetClickedListenerBools(); // Reset input checkers
        if (doDebugLog) Debug.Log("PIPELINE STARTED!");

        // Only enter pipeline if not locked
        while (isSelectCommandSystemLocked)
        {
            yield return null;
        }

        // FIXME: SHOULD IGNORE THE AREA OF THE UI SELECTION!

        #region AWAIT SELECTION OF UNITS

        // AWAIT SELECTION INPUT
        isAwaitingInput = true;
        while (!recievedInput)
        {
            yield return null; // Wait a frame
        }
        // Player clicked
        if (!hasPlayerSelectClicked)
        {
            // Player didnt click select, restart pipeline
            if (doDebugLog) Debug.Log("RESTARTED PIPELINE DUE TO PLAYER NOT CLICKING SELECT");
            isAwaitingInput = false;
            ResetClickedListenerBools(); // Reset input checkers
            StartCoroutine(RestartPipeline());
            yield break;
        }
        isAwaitingInput = false;
        ResetClickedListenerBools();
        // Else, the player did click the select key
        selectionManager.StartTrackingRoutine();
        while (!selectionManager.selectionFinished)
        {
            // Wait until the selection has been finished
            yield return null;
        }
        // Selection was finished, if it resulted in 0, restart the pipeline
        if (selectedUnits.Count <= 0)
        {
            if (doDebugLog) Debug.Log("RESTARTED PIPELINE DUE TO 0 UNITS SELECTED");
            StartCoroutine(RestartPipeline());
            yield break; // Not necessary but just in case
        }

        #endregion

        #region VALIDATE SELECTION COMMANDS

        // Player did select some units, Get the common command for all units selected
        FindCommonCommands();
        // Wait for the player to command click somewhere or restart the pipeline
        isAwaitingInput = true;
        while (!recievedInput)
        {
            yield return null; // Wait a frame
        }
        // Player clicked
        if (!hasPlayerCommandClicked)
        {
            // Player select clicked somewhere else, restart pipeline
            if (doDebugLog) Debug.Log("RESTARTED PIPELINE DUE TO PLAYER CLICKING SELECT SOMEWHERE ELSE AFTER SELECTING");
            isAwaitingInput = false;
            ResetClickedListenerBools(); // Reset input checkers
            StartCoroutine(RestartPipeline());
            yield break;
        }
        // Else, the player clicked the command key
        isAwaitingInput = false;
        ResetClickedListenerBools(); // Reset input checkers

        // Set the Command vector positions for the rest of the pipeline
        mousePosAtCommand = Input.mousePosition;
        mouseWorldPosAtCommand = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Figure out what commands are valid for the location clicked on command
        ValidateCommandsToSend();

        string validatedCommand = "VALID COMMANDS = ";
        foreach (CommandType currCommand in validCommandsOnSelected)
        {
            validatedCommand += currCommand.ToString() + " - ";
        }
        if (doDebugLog) Debug.Log(validatedCommand);

        #endregion

        #region RENDER COMMAND MENU AND AWAIT CHOICE

        // We finished figuring out what commands were valid, render the command menu
        commandMenuManager.StartCommandMenu();

        // Wait for the menu to render first
        while (!commandMenuManager.isCommandMenuRendered)
        {
            yield return null;
        }
        
        // Now wait for player to make a decision
        while (!commandMenuManager.hasPlayerPicked)
        {
            yield return null;
        }
        if (doDebugLog) Debug.Log("PASSED MENU MANAGER PICKED");

        // The player has made a decision, figure out what it was
        if (commandMenuManager.selectedCommand == CommandType.NULL)
        {
            // Player clicked away of the window, restart pipeline
            if (doDebugLog) Debug.Log("RESTARTED PIPELINE DUE TO PLAYER CLICKING AWAY OF THE COMMAND WINDOW");
            StartCoroutine(RestartPipeline());
            yield break;
        }
        // Else, the player selected a valid command from the list

        #endregion

        #region EXECUTION 

        // Player did pick a command from the command menu, issue the order to the units
        if (doDebugLog) Debug.Log("EXECUTING COMMAND = " + commandMenuManager.selectedCommand.ToString());
        foreach (Entity currEntity in selectedUnits)
        {
            if (currEntity.TryGetComponent(out ICommandable command))
            {
                if (doDebugLog) Debug.Log("CALLED THE COMMAND ON = " + currEntity.gameObject.name);
                command.OnCommand(commandMenuManager.selectedCommand, mouseWorldPosAtCommand, entityAtCommandPos);
            }
        }

        #endregion

        // Restart Pipeline upon finishing
        if (doDebugLog) Debug.Log("RESTARTED PIPELINE DUE TO PROPER EXECUTION");
        StartCoroutine(RestartPipeline());
    }

    private IEnumerator RestartPipeline()
    {
        // RESET ANYTHING PERTINENT TO THE UNITS OR COMPONENTS
        selectedUnits.Clear();
        commonCommands.Clear();
        validCommands.Clear();
        validCommandsOnSelected.Clear();
        entityAtCommandPos = null;
        // Stop the coroutine, then restart it
        StopCoroutine(PipelineRoutine);
        PipelineRoutine = StartCoroutine(SelectionToCommandPipeline());
        yield break;
    }

    /// <summary>
    /// Function called to determine what command does all selected units have in common.
    /// Also called by the command menu manager to re-assess options if players command clicks again
    /// </summary>
    public void FindCommonCommands()
    {
        commonCommands = new(allCommands);

        // Find common commands among units
        foreach (Entity entity in selectedUnits) 
        {
            switch (entity.entityType)
            {
                case EntityTypes.BasicBoneman:
                    commonCommands = commonCommands.Intersect(bonemanCommands).ToList(); 
                    break;
                case EntityTypes.Catapult:
                    commonCommands = commonCommands.Intersect(catapultCommands).ToList();
                    break;
                case EntityTypes.ArmoredBoneman:
                    commonCommands = commonCommands.Intersect(armoredCommands).ToList();
                    break;
            }
        }
        // FIXME: HANDLE SELECTION EDGE CASES
        // If more than one unit was selected, stop catapult commands
        if (selectedUnits.Count > 1)
        {
            commonCommands.Remove(CommandType.BoardCatapult);
        }
    }

    /// <summary>
    /// Function called to determine what commands are valid for selection and target
    /// </summary>
    private void ValidateCommands()
    {
        // Clear the valid command list for re-assessment
        validCommands.Clear();

        // Raycast at command placement to figure out target
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosAtCommand, validCommandLayers);
        if (hit)
        {
            // We hit something, figure out if it was an entity
            if (hit.gameObject.TryGetComponent(out Entity entity))
            {
                // If the entity under command is among the selected, avoid any command
                if (selectedUnits.Contains(entity)) return;
                
                // It was an entity, validate commands depending on target
                switch (entity.entityType)
                {
                    case EntityTypes.Catapult:
                        // CHECK IF THE CATAPULT IS BOARDED OR NOT
                        if (!entity.isHolding)
                        {
                            // Catapult is free
                            validCommands.Add(CommandType.BoardCatapult);
                        }
                        break;
                    case EntityTypes.Paladin:
                        validCommands.Add(CommandType.MoveTo);
                        break;
                    case EntityTypes.BasicBoneman:
                        // First contains check avoids self stacking
						validCommands.Add(CommandType.Stack);
                        break;
                }

                // Some cases require only one unit selected
                if (selectedUnits.Count == 1)
                {
                    Entity selectedEntity = selectedUnits.ElementAt(0);
                    // Check what unit was selected and check what relation it has against the entity under the cursor
                    switch (selectedEntity.entityType)
                    {
                        case EntityTypes.BasicBoneman:
                            // Check if the entity under the cursor is a box and we arent grabing anything
                            if (!selectedEntity.isHolding && ( entity.entityType == EntityTypes.Crate || entity.entityType == EntityTypes.Bomb) )
                            {
                                validCommands.Add(CommandType.Grab);
                            }
                            break;
                    }
                }

                // Store a reference to the entity
                entityAtCommandPos = entity;
            }
        }
        else
        {
            // We didnt hit anything
            entityAtCommandPos = null;
            validCommands.Add(CommandType.MoveTo);

            // Now check what unit was selected, handle only singular cases
            if (selectedUnits.Count != 1) return;
            // Else, only one unit was selected, figure out its type
            Entity selectedEntity = selectedUnits.ElementAt(0);
            switch (selectedEntity.entityType)
            {
                case EntityTypes.Catapult:
                    if (selectedEntity.isHolding)
                    {
                        // Add fire and unload command if the catapult is loaded
                        validCommands.Add(CommandType.FireCatapult);
                        validCommands.Add(CommandType.UnloadCatapult);
                    }
                    break;
                case EntityTypes.BasicBoneman:
                    if (selectedEntity.isHolding)
                    {
                        // Add the release command if holding 
                        validCommands.Add(CommandType.Release);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Crosschecks valid commands and possible commands to get a list of usable commands
    /// </summary>
    private void ValidateCommandsOnSelected()
    {
        validCommandsOnSelected = validCommands.Intersect(commonCommands).ToList();
    }

    /// <summary>
    /// Add entity to the selection list
    /// </summary>
    public void AddToSelected(Entity input)
    {
        // Dont add if the selection list is not empty
        if (selectedUnits.Count > 0) return;
        // Else, add only if not already on the list
        if (!selectedUnits.Contains(input)) selectedUnits.Add(input);
    }

    /// <summary>
    /// Helper function to set a boolean for the player select clicking
    /// </summary>
    private void OnSelectClicked()
    {
        // Will only modify the bool if we are expecting input
        if (isAwaitingInput)
        {
            hasPlayerSelectClicked = true;
            recievedInput = true;
        }
    }

    /// <summary>
    /// Helper function to set a boolean for the player command clicking
    /// </summary>
    private void OnCommandClicked()
    {
        if (isAwaitingInput)
        {
            hasPlayerCommandClicked = true;
            recievedInput = true;
        }
    }

    /// <summary>
    /// Helper function to reset the clicker listeners
    /// </summary>
    private void ResetClickedListenerBools()
    {
        recievedInput = false;
        hasPlayerSelectClicked = false;
        hasPlayerCommandClicked = false;
    }

    /// <summary>
    /// Function called by the menu manager if the player command clicks again while
    /// the command window is still open
    /// </summary>
    public void ValidateCommandsToSend()
    {
        ValidateCommands();
        ValidateCommandsOnSelected();
    }

    /// <summary>
    /// <para> Function to lock the player from using the selection and command system. </para>
    /// USE CAREFULLY
    /// </summary>
    public void LockSelectCommandPipeline()
    {
        isSelectCommandSystemLocked = true;
    }

    /// <summary>
    /// <para> Function to unlock the player from using the selection and command system. </para>
    /// USE CAREFULLY
    /// </summary>
    public void UnlockSelectCommandPipeline()
    {
        isSelectCommandSystemLocked = false;
    }
}
