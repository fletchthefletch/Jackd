using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;

    // Reference to the controller
    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private PauseMenu pauseMenu;

    // Current Player Speed
    public float m_speed = 0f;

    // Walk speed
    public float m_walkSpeed = 1.5f;

    // Run speed
    public float m_runSpeed = 3f;

    // Vertical speed
    public float m_speedY = 3f;

    //Jump height
    public float m_jumpHeight = 5f;
    public float m_jumpControlAmount = 2f;

    // Turn Speed
    public float m_turnSpeed = 300f;

    // Movement direction of the player
    private Vector3 movement, movementJump;

    // Player input
    private Vector3 inputDirection;

    // Gravity
    private const float GRAVITY = 9.86f;
    private const float GRAVITY_STICK = 0.3f;

    private Transform playerCam;

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
    private Camera cam;

    private PlayListCycler playlist;

    private bool isClimbing = false;
    private float maxClimbHeight = 92f;
    private float climbRate = 2f;
    private MainGame game;
    private bool hasWon = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //Fetch the Animator component
        animator = GetComponent<Animator>();

        // Get player
        player = GetComponent<Player>();

        // Get game
        game = FindObjectOfType<MainGame>();

        if (game == null)
        {
            Debug.Log("Could not retrieve game for player instance");
        }
        // Get main camera
        playerCam = cam.transform;
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
    public void playClimb()
    {
        // Play climbing sound
    }

    void Update()
    {
        if (transform.position.y > maxClimbHeight)
        {
            if (!hasWon)
            {
                // Player has won
                // Stop climbing animation
                // Stop climbing sound
                game.playerCompletedObjective();
                animator.SetBool("isClimbing", false);
                animator.SetBool("victory", true);
                hasWon = true;
            }
            return;
        }

        if (pauseMenu.isOpen())
        {
            animator.SetFloat("speed", 0f);
            return; // Do not update player and state
        }
        // Update current player state and movement
        UpdatePlayerState();
        UpdateHorizontalMovement();
        if (isClimbing && transform.position.y < maxClimbHeight)
        {
            UpdateClimb();
            return;
        }
        UpdateVerticalMovement();
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
    public void startStopClimbing(bool start)
    {
        isClimbing = start;
        if (start && transform.position.y < maxClimbHeight)
        {
            // Climbing animation
            // Climbing sound
            animator.SetBool("isClimbing", true);
        }
        
        else
        {
            // Stop climbing animation
            // Stop climbing sound
            animator.SetBool("isClimbing", false);
        }
    }
    private void UpdateClimb()
    {
        // Detect the players height here - don't climb further than the cloud!
        m_speedY = climbRate;
        movementJump.y = m_speedY;
        controller.Move(movement * Time.deltaTime + movementJump * Time.deltaTime);
        playClimb();
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
        animator.SetFloat("speed", m_speed);
    }


    private void UpdateVerticalMovement()
    {

        if (controller.isGrounded)
        {
            m_speedY = -GRAVITY * GRAVITY_STICK;
            animator.SetBool("jump", false);

            //If we press space and not already jumping and if we are not crawling or crouching, then jump
            if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("jump")) 
            {
                // Jump
                m_speedY = m_jumpHeight;
                animator.SetBool("jump", true);
                m_moveState = MoveState.Jump;       
            }
        }
        else
        {
            m_moveState = MoveState.Jump;
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
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    // Run
                    m_speed = m_runSpeed;
                    m_moveState = MoveState.Run;
                    animator.SetBool("running", true);
                } 
                else
                {
                    // Walk
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
