using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum CommandType
{
    MoveTo,
    Carry,
    ReleaseCarry,
    Stack,
    BoardCatapult,
    UnloadCatapult,
    FireCatapult,
    Attack
}

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    private List<Entity> selectedUnits = new();

    private List<Entity> activeUnits = new();

    // Command list for all different types of entities, shouldnt be modified
    private List<CommandType> allCommands = new();
    private List<CommandType> bonemanCommands = new();
    private List<CommandType> armoredCommands = new();
    private List<CommandType> catapultCommands = new();

    // Local Variables
    List<CommandType> commonCommands = new();
    List<CommandType> validCommands = new();
    List<CommandType> validCommandsOnSelected = new();

    private void Awake()
    {
        // Handle singleton
        if (instance != null) Destroy(gameObject);
        instance = this;

        // Subscribe to input events
        InputManager.OnCommandPressed += HandleCommand;

        #region ALL COMMANDS
        allCommands.Add(CommandType.Carry);
        allCommands.Add(CommandType.MoveTo);
        allCommands.Add(CommandType.Stack);
        allCommands.Add(CommandType.BoardCatapult);
        allCommands.Add(CommandType.UnloadCatapult);
        allCommands.Add(CommandType.FireCatapult);
        allCommands.Add(CommandType.Attack);
        allCommands.Add(CommandType.ReleaseCarry); 
        #endregion

        #region BONEMAN COMMANDS
        bonemanCommands.Add(CommandType.Carry);
        bonemanCommands.Add(CommandType.ReleaseCarry);
        bonemanCommands.Add(CommandType.MoveTo);
        bonemanCommands.Add(CommandType.Stack);
        bonemanCommands.Add(CommandType.BoardCatapult);
        #endregion

        #region ARMORED BONEMAN COMMANDS
        armoredCommands.Add(CommandType.MoveTo);
        armoredCommands.Add(CommandType.BoardCatapult);
        armoredCommands.Add(CommandType.Attack);
        #endregion

        #region CATAPULT COMMANDS
        catapultCommands.Add(CommandType.MoveTo);
        catapultCommands.Add(CommandType.UnloadCatapult);
        catapultCommands.Add(CommandType.FireCatapult);
        #endregion
    }

    private void OnDestroy()
    {
        // Unsubscribe from input events
        InputManager.OnCommandPressed -= HandleCommand;
    }

    /// <summary>
    /// Function will be called upon clicking the command button
    /// </summary>
    private void HandleCommand()
    {
        // Dont do anything if the selected list is empty
        if (selectedUnits.Count <= 0) return;

        // Get the common command for all units selected
        FindCommonCommands();
        // Figure out what commands are valid for the location clicked
        ValidateCommands();
        // Figure out what commands are valid out of the list of possible ones
        ValidateCommandsOnSelected();

        string validatedCommand = "VALID COMMANDS = ";
        foreach (CommandType currCommand in validCommandsOnSelected)
        {
            validatedCommand += currCommand.ToString() + " - ";
        }
        Debug.Log(validatedCommand);

        commonCommands.Clear();
        validCommands.Clear();
        validCommandsOnSelected.Clear();
    }

    /// <summary>
    /// Function called to determine what command does all selected units have in common
    /// </summary>
    private void FindCommonCommands()
    {
        commonCommands = new(allCommands);

        // Find common commands among units
        foreach (Entity entity in selectedUnits) 
        {
            switch (entity.entityType)
            {
                case EntityTypes.Boneman:
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
        // Raycast at cursor to figure out target
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit)
        {
            // We hit something, figure out if it was an entity
            if (hit.collider.gameObject.TryGetComponent(out Entity entity))
            {
                // It was an entity, validate commands depending on target
                switch (entity.entityType)
                {
                    case EntityTypes.Catapult:
                        validCommands.Add(CommandType.BoardCatapult);
                        validCommands.Add(CommandType.MoveTo);
                        break;
                    case EntityTypes.SmallEnemy:
                        validCommands.Add(CommandType.Attack);
                        validCommands.Add(CommandType.MoveTo);
                        break;
                }

            }
        }
        else
        {
            // We didnt hit anything, should be a valid MoveTo Command
            validCommands.Add(CommandType.MoveTo);
        }
    }

    /// <summary>
    /// Crosschecks valid commands and possible commands to get a list of issuable commands
    /// </summary>
    private void ValidateCommandsOnSelected()
    {
        validCommandsOnSelected = validCommands.Intersect(commonCommands).ToList();
    }

    /// <summary>
    /// Function to be called to tell selected units to perform command
    /// </summary>
    private void IssueCommand()
    {

    }

    /// <summary>
    /// Clear all selected units
    /// </summary>
    public void ClearSelected() => selectedUnits.Clear();

    /// <summary>
    /// Add entity to the selection list
    /// </summary>
    public void AddToSelected(Entity input)
    {
        if (!selectedUnits.Contains(input)) selectedUnits.Add(input);
    }
}
