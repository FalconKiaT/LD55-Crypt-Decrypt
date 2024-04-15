/*
	Created by @DawnosaurDev at youtube.com/c/DawnosaurStudios
	Thanks so much for checking this out and I hope you find it helpful! 
	If you have any further queries, questions or feedback feel free to reach out on my twitter or leave a comment on youtube :D

	Feel free to use this in your own games, and I'd love to see anything you make!
 */

/* IAN:
 * I performed some modifications to this script to adapt it to our game
 */

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using FKTools;

public class PlayerController : FKMonoBehaviour
{
    //Scriptable object which holds all the player's movement parameters. If you don't want to use it
    //just paste in all the parameters, though you will need to manuly change all references in this script

    //HOW TO: to add the scriptable object, right-click in the project window -> create -> Player Data
    //Next, drag it into the slot in playerMovement on your player

    [Header("Sound Effects")]
    [SerializeField] private FMODUnity.EventReference jumpSound;
    [SerializeField] private FMODUnity.EventReference walkingSound;

    [Header("Player Movement Scriptable Obj")]
    public PlayerMoveData Data;

    #region Variables
    //Components
    public Rigidbody2D RB { get; private set; }
    private Animator animator;

    //Variables control the various actions the player can perform at any time.
    //These are fields which can are public allowing for other sctipts to read them
    //but can only be privately written to.
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool isGrounded{ get; private set; }

    //Timers (also all fields, could be private and a method returning a bool could be used)
    public float LastOnGroundTime { get; private set; }

    //Jump
    private bool _isJumpCut;
    private bool _isJumpFalling;

    // Variables
    private Vector2 _moveInput;
    public float LastPressedJumpTime { get; private set; }

    // Animator
    private int lastDirectionVal = 1;

    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _collidableLayers;
    #endregion

    private void Awake()
    {
        // Get components
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Subscribe to input events
        InputManager.OnJumpPressed += OnJumpInput;
        InputManager.OnJumpReleased += OnJumpUpInput;

        // Pause managment
        FKAnimator.AddToPauseList(animator);
    }

    private void Start()
    {
        SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
    }

    private void OnDestroy()
    {
        // Unsubscribe from input events
        InputManager.OnJumpPressed -= OnJumpInput;
        InputManager.OnJumpReleased -= OnJumpUpInput;
        // Remove animator from pause manager
        FKAnimator.RemoveFromPauseList(animator);
    }

    public override void FKUpdatePauseAware()
    {
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        #endregion

        #region HANDLE DIRECTION
        _moveInput.x = InputManager.movementInput.x;
        _moveInput.y = InputManager.movementInput.y;

        if (_moveInput.x != 0)
            CheckDirectionToFace(_moveInput.x > 0);

        #endregion

        #region COLLISION CHECKS
        if (!IsJumping)
        {
            // Check if the player is touching the ground or something that they can stand on
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _collidableLayers)) //checks if set box overlaps with ground
            {
                if (LastOnGroundTime < -0.1f)
                {
                    isGrounded = true;
                }

                LastOnGroundTime = Data.coyoteTime; // if so sets the lastGrounded to coyoteTime
            }
        }
        else
        {
            isGrounded = false;
        }
        #endregion

        #region JUMP CHECKS

        if (IsJumping && RB.velocity.y < 0.01f)
        {
            // Player started to fall
            IsJumping = false;
        }
        
        if (LastOnGroundTime > 0 && !IsJumping)
        {
            // 
            _isJumpCut = false;

            if (!IsJumping)
                _isJumpFalling = false;
        }

        // Jump if its input was pressed
        if (CanJump() && LastPressedJumpTime > 0)
        {
            SoundManager.instance.PlaySound(jumpSound);
            IsJumping = true;
            _isJumpCut = false;
            _isJumpFalling = false;
            Jump();
        }
        #endregion

        #region GRAVITY
        // Highly increase gravity if the player is holding the down input (down arrow/S Key)
        if (RB.velocity.y < 0 && _moveInput.y < 0)
        {
            SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
        }
        // Increase gravity if jump button is released mid fall
        else if (_isJumpCut)
        {
            SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
        }
        // Player is holding down the jump input, slow down falling
        else if ((IsJumping|| _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
        }
        else if (RB.velocity.y < 0)
        {
            // Higher gravity if falling, no other 
            SetGravityScale(Data.gravityScale * Data.fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
        }
        else
        {
            //Default gravity if standing on a platform or moving upwards
            SetGravityScale(Data.gravityScale);
        }
        #endregion

        #region ANIMATOR

        // Set the direction of the animator
        if (IsFacingRight) lastDirectionVal = 1;
        else lastDirectionVal = -1;
        animator.SetFloat("directionX", lastDirectionVal);

        // Check if player is jumping or falling
        if (IsJumping)
        {
            // Jumping
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
            animator.SetBool("isRunning", false);
            return;
        }
        else if (!isGrounded)
        {
            // Falling
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
            animator.SetBool("isRunning", false);
            return;
        }

        // Player was grounded, check if the player is at idle or running
        if (Mathf.Abs(RB.velocity.x) > 0.01f)
        {
            // Running
            animator.SetBool("isRunning", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            return;
        }
        else
        {
            // Idle
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            return;
        }

        #endregion
    }

    public override void FKFixedUpdatePauseAware()
    {
        //Handle Run
        Run(1);
    }

    #region INPUT CALLBACKS
    //Methods which handle input detected in Update()

    // On jump pressed
    public void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }

    // On Jump released
    public void OnJumpUpInput()
    {
        if (CanJumpCut())
            _isJumpCut = true;
    }
    #endregion

    #region GENERAL METHODS
    // Change the scale of gravity to affect how fast the player falls
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }
    #endregion

    //MOVEMENT METHODS
    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        #endregion

        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

        /*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
    }

    #endregion

    #region JUMP METHODS
    private void Jump()
    {
        //Ensures we can't call Jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = Data.jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        #endregion
    }

    #endregion

    #region CHECK METHODS
    // Change bool if player turns
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            IsFacingRight = !IsFacingRight;
    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }

    private bool CanJumpCut()
    {
        return IsJumping && RB.velocity.y > 0;
    }

    #endregion


    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.color = Color.blue;
    }
    #endregion
}