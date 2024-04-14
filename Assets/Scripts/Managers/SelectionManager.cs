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

    // Local variables
    private Vector2 initialMousePos;
    private RectTransform managerRect;
    private Coroutine selectTrackingRoutine = null;
    private Collider2D[] selectionCast;

    private void Awake()
    {
        // Get components
        managerRect = GetComponent<RectTransform>();

        // Subscribe to input events
        InputManager.OnSelectToggled += HandleTrackingRoutine;

        // Make sure Selection box is not visible
        selectionBox.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // Unsubscribe from input events
        InputManager.OnSelectToggled -= HandleTrackingRoutine;

        // Make sure Selection box is not visible
        selectionBox.gameObject.SetActive(false);
    }

    // Handle the coroutine that tracks the selection of the player
    private void HandleTrackingRoutine(bool isPressed)
    {
        if (isPressed)
        {
            // Player has pressed the key, handle routine
            if (selectTrackingRoutine == null)
            {
                initialMousePos = Input.mousePosition;
                selectTrackingRoutine = StartCoroutine(TrackSelection());
            }
            return;
        }
        // Else, the player has released the select button
        StopCoroutine(selectTrackingRoutine);
        // Ended selection, remove box
        selectionBox.gameObject.SetActive(false);
        selectTrackingRoutine = null;
        // Clear the selection array
        UnitManager.instance.ClearSelected();
        // Get all under selection
        Vector2 initialMousePosWorld = Camera.main.ScreenToWorldPoint(initialMousePos);
        Vector2 finalMousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectionCast = Physics2D.OverlapAreaAll(initialMousePosWorld, finalMousePosWorld, selectableLayers, -0.1f, 0.1f);
        // If the cast resulted in 0, then dont add any
        if (selectionCast.Length <= 0) return;
        // Else, add all possible selections to the array
        foreach (Collider2D currCollider in selectionCast)
        {
            // If the entity implements selectable, call it
            if (currCollider.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnSelected();
                Debug.Log("ADDED = " + currCollider.name);
            }
        }
    }

    // Routine that tracks the position of the player's mouse
    private IEnumerator TrackSelection()
    {
        bool dragDistancePassed = false;

        // Continuously check for distance
        while (true)
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
    }

    // Handles the selection box object
    private void UpdateSelectionBox()
    {
        float width = Input.mousePosition.x - initialMousePos.x;
        float height = Input.mousePosition.y - initialMousePos.y;

        // Set size
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        // Set position
        selectionBox.anchoredPosition = new Vector2(initialMousePos.x, initialMousePos.y) + new Vector2(width/2,height/2);
    }
}
