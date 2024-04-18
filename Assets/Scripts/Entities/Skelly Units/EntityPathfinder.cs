using UnityEngine;
using FKTools;

public class EntityPathfinder : FKMonoBehaviour
{
    [Header("Important Locations")]
    public GameObject gapDetector;
    public GameObject jumpDetector;

    [Header("Settings")]
    public float speed;
    public float jumpForce;
    // DEBUG HIDE
    [HideInInspector] public float destinationDelta = 1f;

    [Header("Jumping READ ONLY")]
    public bool canJump;
    public bool jumping;

    [Header("Movement READ ONLY")]
    public bool canMove = true;
    public int collisionCounter = 0;
    public bool startMoving;

    // Local Variables
    private float jumpColliderXPos;
    private Animator skeletonAnimator;
    private Rigidbody2D rb;
    [HideInInspector] public Vector3 placeToMove;

    private void Start()
    {
        // Get Components
        skeletonAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // FIXME: if default value in animator is 0, it wont fix the box until first move
        int sign = (int)Mathf.Sign(skeletonAnimator.GetFloat("directionX"));
        jumpColliderXPos = jumpDetector.transform.localPosition.x * sign;
    }

    // Update is called once per frame
    public override void FKFixedUpdatePauseAware()
    {
        // changes collider position
        //transform.position = transform.position + new Vector3(0, 0, 0);
        //gapDetector.transform.position = transform.position + new Vector3(0, -1, 0);
        if (startMoving)
        {
            CheckDirection();
            MoveToPlace();
        }
        else
        {
            skeletonAnimator.SetBool("isMoving", false);
        }
    }
    public void CheckDirection()
    {
        CollisionChecker();

        // if the skelly encounters a wall
        if (!canMove && jumpDetector.GetComponent<SkellyCollisionHandler>().getMove() && !jumping)
        {
            canMove = true;
            canJump = true;
        }
        else if (!canMove && !jumping)
        {
            Debug.Log("ENTERED THE LAST IF");
            startMoving = false;
        }
    }

    public void MoveToPlace()
    {
        // determines if the skelly has arrived with a float delta
        if (placeToMove.x - destinationDelta < transform.position.x && transform.position.x < placeToMove.x + destinationDelta)
        {
            startMoving = false;
            canMove = false;
        }

        if (!canMove) return;

        // starts the animator
        skeletonAnimator.SetBool("isMoving", true);
        // Determine direction
        float direction = Mathf.Sign(placeToMove.x - transform.position.x);
        skeletonAnimator.SetFloat("directionX", -direction);

        // Switch the jump collider from one side to another
        if ((int)direction == -1)
        {
            // Going left
            jumpDetector.transform.localPosition = new Vector2(-jumpColliderXPos, jumpDetector.transform.localPosition.y);
        }
        else
        {
            // Going right
            jumpDetector.transform.localPosition = new Vector2(jumpColliderXPos, jumpDetector.transform.localPosition.y);
        }

        // determines what the skeleton is doing
        if (canJump && !jumping)
        {
            // Jump using physics
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Force);
            jumping = true;
            canJump = false;
        }
        // Start moving towards target
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime * Vector2.right));

        // stops the jump
        if (!gapDetector.GetComponent<SkellyCollisionHandler>().getMove())
        {
            jumping = false;
        }
    }

    private void CollisionChecker()
    {
        if (collisionCounter == 0)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }

    public void StartPathfinding(Vector3 targetPosition)
    {
        startMoving = true;
        placeToMove = targetPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            collisionCounter++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            collisionCounter--;
        }
    }
}
