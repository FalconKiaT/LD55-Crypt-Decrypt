using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenuManager : MonoBehaviour
{
    // Singleton
    public static CommandMenuManager instance;

    // boolean that will change to true upon the player making a click, and false after another one
    public bool hasPlayerPicked;
    public CommandType selectedCommand = CommandType.NULL;

    [Header("SETTINGS")]
    [SerializeField] private RectTransform commandMenu;
    [SerializeField] private RectTransform backgroundImage;
    [SerializeField] private float optionsOffset;

    [Header("MENU OPTIONS")]
    [SerializeField] private RectTransform nullOption;
    [SerializeField] private RectTransform moveToOption;
    [SerializeField] private RectTransform carryToOption;
    [SerializeField] private RectTransform releaseOption;
    [SerializeField] private RectTransform stackOption;
    [SerializeField] private RectTransform boardOption;
    [SerializeField] private RectTransform unloadOption;
    [SerializeField] private RectTransform fireOption;
    [SerializeField] private RectTransform attackOption;

    // Local variables
    private bool isCommandMenuRendered;
    private Coroutine menuSelection;
    private RectTransform startingPos;
    private Dictionary<CommandType, RectTransform> menuOptions = new();


    private void Awake()
    {
        // Handle singleton
        if (instance != null) Destroy(gameObject);
        instance = this;

        // Variables
        startingPos = Instantiate(nullOption); // Copy

        // Populate dictionary
        menuOptions.Add(CommandType.MoveTo, moveToOption);
        menuOptions.Add(CommandType.Carry, carryToOption);
        menuOptions.Add(CommandType.ReleaseCarry, releaseOption);
        menuOptions.Add(CommandType.Stack, stackOption);
        menuOptions.Add(CommandType.BoardCatapult, boardOption);
        menuOptions.Add(CommandType.UnloadCatapult, unloadOption);
        menuOptions.Add(CommandType.FireCatapult, fireOption);
        menuOptions.Add(CommandType.Attack, attackOption);

        // Subscribe to input
        InputManager.OnSelectClicked += ToggleClick;
        //InputManager.OnCommandPressed += RenderCommandMenu;
    }

    private void OnDestroy()
    {
        // Handle singleton
        instance = null;

        // Unsubscribe from input
        InputManager.OnSelectClicked -= ToggleClick;
        //InputManager.OnCommandPressed -= RenderCommandMenu;
    }

    private void HandleMenuSelectionRoutine()
    {
        if (menuSelection != null)
        {
            menuSelection = StartCoroutine(MenuSelectionRoutine());
        }
    }

    private IEnumerator MenuSelectionRoutine()
    {
        // Wait until the player either clicks a choice or clicks away
        while (!hasPlayerPicked)
        {
            yield return null;
        }
        // Wait another frame so the button registers
        yield return null;
        // Stop rendering menu
        UnactivateAllOptions();
        commandMenu.gameObject.SetActive(false);
    }

    private void ToggleClick()
    {
        if (isCommandMenuRendered)
        {
            hasPlayerPicked = true;
        }
    }

    public void RenderCommandMenu()
    {
        isCommandMenuRendered = true;
        hasPlayerPicked = false;

        // Render the command menu at the mouse position
        commandMenu.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        commandMenu.gameObject.SetActive(true);

        // Render all valid options into the command menu
        foreach (CommandType currCommand in UnitManager.instance.validCommandsOnSelected)
        {
            if (menuOptions.TryGetValue(currCommand, out RectTransform rct))
            {
                rct.anchoredPosition = new Vector2(startingPos.anchoredPosition.x, startingPos.anchoredPosition.y);
                startingPos.anchoredPosition = new Vector2(startingPos.anchoredPosition.x, startingPos.anchoredPosition.y - optionsOffset);
                rct.gameObject.SetActive(true);
            }
        }
        // FIXME: CHANGE BACKGROUND HEIGHT
        // FIXME: HANDLE NO OPTIONS TOO
        HandleMenuSelectionRoutine();
    }

    /// <summary>
    /// Will execute the command selected in the menu
    /// </summary>
    public void ExecuteCommand(CommandOption input)
    {
        selectedCommand = input.getCommand();
        Debug.Log("EXECUTED COMMAND = " + selectedCommand.ToString());
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
        attackOption.gameObject.SetActive(false);
    }
}
