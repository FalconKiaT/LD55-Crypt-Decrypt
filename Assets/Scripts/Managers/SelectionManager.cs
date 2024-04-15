using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FKTools;
using UnityEngine.UI;

/// <summary>
/// Class that handles the drag click selection, should be in an object nested in the canvas
/// </summary>
public class SelectionManager : FKMonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private RectTransform selectionBox;

    // SelectionMagnitude: Controls how far away the mouse needs to get for the selection box to render
    [SerializeField] private float dragSelectDistance;
    [SerializeField] private LayerMask selectableLayers;

    // Interface bools with Unit Manager
    [HideInInspector] public bool selectionFinished = false;

    // Local variables
    private bool isSelectHeldDown;
    private Vector2 initialMousePos;
    private RectTransform managerRect;
    private Coroutine selectTrackingRoutine = null;
    private Collider2D[] selectionCast;

    private void Awake()
    {
        // Get components
        managerRect = GetComponent<RectTransform>();

        // Make sure Selection box is not visible
        selectionBox.gameObject.SetActive(false);

        // Subscribe to input
        InputManager.OnSelectToggled += SetSelectHeldDownBool;
    }

    private void OnDestroy()
    {
        // Make sure Selection box is not visible
        selectionBox.gameObject.SetActive(false);

        // Unsubscribe to input
        InputManager.OnSelectToggled -= SetSelectHeldDownBool;
    }

    // Handle the coroutine that tracks the selection of the player
    public void StartTrackingRoutine()
    {
        // Player has pressed the key, handle routine
        if (selectTrackingRoutine == null)
        {
            selectionFinished = false;
            initialMousePos = Input.mousePosition;
            selectTrackingRoutine = StartCoroutine(TrackSelection());
        }
    }

    // Routine that tracks the position of the player's mouse
    private IEnumerator TrackSelection()
    {
        bool dragDistancePassed = false;
        // Event system will turn this bool into false if the players lets go of the select
        // It starts as true because we already check for the click on the unit manager
        isSelectHeldDown = true;

        // Player started clicking select, continuously check for distance to start rendering box
        while (isSelectHeldDown)
        {
            // Check for the distance
            if (!dragDistancePassed && (initialMousePos - (Vector2)Input.mousePosition).magnitude > dragSelectDistance)
            {
                // Did pass distance
                dragDistancePassed = true;
            }
            else if (!dragDistancePassed)
            {
                // Havent passed distance yet
                yield return null;
                continue;
            }

            // if drag distance was passed, render the selection box
            UpdateSelectionBox();

            // Set the selection box active
            selectionBox.gameObject.SetActive(true);
            yield return null;
        }
        // Player has released the select button
        selectionBox.gameObject.SetActive(false);
        // Get all under selection
        Vector2 initialMousePosWorld = Camera.main.ScreenToWorldPoint(initialMousePos);
        Vector2 finalMousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectionCast = Physics2D.OverlapAreaAll(initialMousePosWorld, finalMousePosWorld, selectableLayers, -0.1f, 0.1f);
        if (selectionCast.Length <= 0)
        {
            // If the cast resulted in 0, then dont add any
            StartCoroutine(ExitSelectionRoutine());
            yield break;
        }
        // Else, add all possible selections to the array
        foreach (Collider2D currCollider in selectionCast)
        {
            // If the entity implements selectable, call it
            if (currCollider.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnSelected();
                if (UnitManager.instance.doDebugLog) Debug.Log("ADDED = " + currCollider.name);
            }
        }
        // Finished
        StartCoroutine(ExitSelectionRoutine());
    }

    private IEnumerator ExitSelectionRoutine()
    {
        StopCoroutine(selectTrackingRoutine);
        selectTrackingRoutine = null;
        selectionFinished = true;
        yield break;
    }

    // Handles the selection box object
    private void UpdateSelectionBox()
    {
        float width = Input.mousePosition.x - initialMousePos.x;
        float height = Input.mousePosition.y - initialMousePos.y;

        // Set size
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        // Set position
        selectionBox.anchoredPosition = new Vector2(initialMousePos.x, initialMousePos.y) + new Vector2(width / 2, height / 2);
    }

    /// <summary>
    /// Function managed by input
    /// </summary>
    private void SetSelectHeldDownBool(bool isPressed)
    {
        // Only listen to when the player releases the click
        if (isSelectHeldDown && !isPressed)
        {
            // Player released the select key
            isSelectHeldDown = false;
        }
    }
}
