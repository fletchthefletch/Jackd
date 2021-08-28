using UnityEngine;

public class Challenge_Advanced_PlayerController : MonoBehaviour
{
    private Player player;

    // Reference to the controller
    public CharacterController controller;

    // Reference to the Animator component
    public Animator animator;

    [SerializeField]
    private PauseMenu pauseMenu;

    // Current Player Speed
    public float m_speed = 10;

    // Walk speed
    public float m_walkSpeed = 2;

    // Run speed
    public float m_runSpeed = 6;

    // Vertical speed
    public float m_speedY;

    //Jump height
    public float m_jumpHeight = 10;
    public float m_jumpControlAmount = 3f;

    // Turn Speed
    public float m_turnSpeed;

    // Movement direction of the player
    private Vector3 movement, movementJump;

    // Player input
    private Vector3 inputDirection;

    // Gravity
    private const float GRAVITY = 9.86f;
    private const float GRAVITY_STICK = 0.3f;

    private Transform playerCam;
    private Challenge_Advanced_CameraController m_cameraController;

    // Player state
    public enum MoveState
    {
        Idle,
        Walk,
        Run,
        Jump,
    };

    // Current player state
    [SerializeField] 
    private MoveState m_moveState;

    [SerializeField] 
    private float smoothSpeed;

    // Main game camera
    [SerializeField] 
    Camera cam;

    private PlayListCycler playlist;

    void Start()
    {
        //Fetch the CharacterController component 
        controller = GetComponent<CharacterController>();

        //Fetch the Animator component
        animator = GetComponent<Animator>();

        // Get player
        player = GetComponent<Player>();

        // Get main camera
        playerCam = cam.transform;

        //Get the Camera Controller component 
        m_cameraController = playerCam.GetComponent<Challenge_Advanced_CameraController>();

        m_moveState = MoveState.Idle;

        // Get playlist
        playlist = FindObjectOfType<PlayListCycler>();
    }
    public void playStep()
    { 
        playlist.playPlayerSound("Step", true);
    }
    public void playJump()
    {
        playlist.playPlayerSound("Jump", true);
    }

    void Update()
    {
        if (pauseMenu.isOpen())
        {
            return; // Do not update player rotation and state
        }
            // Update current player state
            UpdatePlayerState();

            // Left-Right, Forward-Backward movement
            UpdateHorizontalMovement();

            // Jump 
            UpdateVerticalMovement();

            // Rotate player
            UpdateRotation();

            // Move   
            controller.Move(movement * Time.deltaTime + movementJump * Time.deltaTime);
    }

    private void UpdateRotation()
    {
        if (inputDirection != Vector3.zero)            
        {
            //Calcuate the angle required to rotate based on the given direction
            float turnAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0 , turnAngle, 0);

            // Smoothly Interpolate from current rotation to target rotation with the given turn speed amount 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_turnSpeed * Time.deltaTime);           
        }
    }

    private void UpdateHorizontalMovement()
    {
        // Receive user input
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical")).normalized;
   
        // New target vector
        movement.x = inputDirection.x * m_speed;
        movement.z = inputDirection.z * m_speed;
        movement = inputDirection.normalized * m_speed;
        movement = (playerCam.forward * inputDirection.z + playerCam.right * inputDirection.x) * m_speed;       
    }


    //Handles the jumping functionality
    private void UpdateVerticalMovement()
    {
        //Player is on ground, then only check for jump input
        if (controller.isGrounded)
        {
            //Common hack used to make sure the player is always sticked to the ground when on ground
            //Sometimes, isgrounded does not work correctly and the player keeps floating in air
           m_speedY = -GRAVITY * GRAVITY_STICK;
        
           animator.SetBool("jump", false);

            //If we press space and not already jumping and if we are not crawling or crouching, then jump
            if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("jump")) 
            {
                //Simple jump
                //m_speedY = m_jumpHeight;

                //More Controlled jump (proportionate to our defined gravity value)
                movement.y += Mathf.Sqrt(m_jumpHeight * m_jumpControlAmount * GRAVITY);

                animator.SetBool("jump", true);
                m_moveState = MoveState.Jump;       
            }
        }
        //Player is airborne
        else
        {
            m_moveState = MoveState.Jump;
            //Apply gravity so that the player falls back to the ground
            m_speedY -= GRAVITY * Time.deltaTime;
        }      
        movementJump.y = m_speedY;
    }

    private void UpdatePlayerState()
    {
        //If grounded 
        if (controller.isGrounded)
        {
            animator.SetBool("jump", false);

            //If we have some input then check for Run or Walk states
            if (inputDirection != Vector3.zero)
            {
                // Walk when key pressed
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    m_speed = m_runSpeed;
                    m_moveState = MoveState.Run;
                    animator.SetBool("running", true);
                } 
                else
                {
                    m_speed = m_walkSpeed;
                    m_moveState = MoveState.Walk;
                    animator.SetBool("running", false);
                }
            }
            else 
            {
                // Idle
                m_speed = 0; 
                m_moveState = MoveState.Idle;
                animator.SetBool("running", false);
            }   
        } 
    }
}
