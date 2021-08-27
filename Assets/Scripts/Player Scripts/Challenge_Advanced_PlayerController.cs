using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Challenge_Advanced_PlayerController : MonoBehaviour
{
    private Player player;

    //Reference to the CharacterController component
    public CharacterController controller;

    //Reference to the Animator component
    public Animator animator;

    [SerializeField]
    private PauseMenu pauseMenu;

    //Current Player Speed
    public float m_speed = 10;

    //Walk speed
    public float m_walkSpeed = 2;

    //Run speed
    public float m_runSpeed = 6;

    //Crawl speed
    public float m_crawlSpeed = 1;

    //Vertical speed
    public float m_speedY;

    //Jump height
    public float m_jumpHeight = 10;

    //Used for more controlled jump
    public float m_jumpControlAmount = 2f;

    //Current Speed at which player smoothly rotates
    public float m_turnSpeed;

    //Movement direction of the player
    private Vector3 movement, movementJump;

    //Player input
    private Vector3 inputDirection;

    //Gravity
    private const float GRAVITY = 9.87f;

    //Gravity stick constant
    private const float GRAVITY_STICK = 0.3f;

    //Reference to the camera
    private Transform playerCam;

    private Challenge_Advanced_CameraController m_cameraController;

    private bool m_isAttackAiming = false;

    //Player state
    public enum MoveState
    {
        Idle,
        Walk,
        Crouch,
        Crawl,
        Run,
        Jump,
    };

    //Current player state
    [SerializeField] MoveState m_moveState;

    //Smoothing for changing between speeds (can modify later with acceleration and decceleration for more control)
    [SerializeField] private float smoothSpeed;

    //Reference to the crosshair image
    //public UnityEngine.UI.Image m_crossHair;


    //Attack ray while shooting
    private Ray m_fireRay;

    //Stores what was hit?
    private RaycastHit m_hitInfo;

    [SerializeField] Camera cam;

    private PlayListCycler playlist;

    void Start()
    {
        //Fetch the CharacterController component 
        controller = GetComponent<CharacterController>();

        //Fetch the Animator component
        animator = GetComponent<Animator>();

        // Get player
        player = GetComponent<Player>();

        //Fetch the Main camera and store its reference
        //Caching this reference is extremely cheap since Camera.main uses Gameobject.Find 
        //Which is very expensive if used in Update
        // playerCam = Camera.main.transform.parent.parent;
        playerCam = cam.transform;

        //Get the Camera Controller component 
        m_cameraController = playerCam.GetComponent<Challenge_Advanced_CameraController>();

        m_moveState = MoveState.Idle;

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
            //Update current player state
            UpdatePlayerState();

            //Updates to check if player is aiming to attack
            //UpdateAttackAim();

            //Updates to check if player is attacking
            // UpdateAttack();

            //Left-Right, Forward-Backward movement
            UpdateHorizontalMovement();

            //Jump 
            UpdateVerticalMovement();

            //Rotate Face in the movement direction
            UpdateRotation();

            //Updates the audio based on player movement
            //UpdateAudio();

            //Move in the given input direction and in the jump direction if there is any        
            controller.Move(movement * Time.deltaTime + movementJump * Time.deltaTime);
        
    }


    private void UpdateAttack()
    {   
        //Attack only if aiming
        if(m_isAttackAiming)
        {   
            //Get left click
            if(Input.GetMouseButtonDown(0))
            {   
                //Debug.DrawLine(m_fireRay.origin, m_fireRay.direction * 100f, Color.red, 3);
                Debug.DrawRay(m_fireRay.origin, m_fireRay.direction * 100, Color.red, 10);

                if(Physics.Raycast(m_fireRay, out m_hitInfo))
                {

                    Debug.Log($"Target hit: {m_hitInfo.transform.name}");
                }

                //Instantiate muzzle flash
                //Instantiate(m_muzzleFlash, m_muzzlePos);

                //Play the gun shot clip
                //gunshotsAudioManager.PlayRandomOneShot();
            }
        }
 
        
    }

    //Updates the attack state and attack animation
    private void UpdateAttackAim()
    {   
        //If Right click when not crawling then move to attack state
        if(Input.GetMouseButton(1) && m_moveState != MoveState.Crawl)
        {   
            //activate the crosshair during aim
            //m_crossHair.CrossFadeAlpha(1,0.1f,false);
            m_isAttackAiming = true;
            Cursor.lockState = CursorLockMode.Locked;
          
            //Calculate the direction from camera to the crosshair to fire a ray
           // Vector3 m_attackDirection = Camera.main.ScreenToViewportPoint(m_crossHair.rectTransform.position);
           // m_fireRay = Camera.main.ViewportPointToRay(m_attackDirection);

            //Activate the aiming animation layer by increasing its weight so that it overrides the base layer
            animator.SetLayerWeight(1,1);
            animator.SetBool("attackAim", m_isAttackAiming);
            animator.SetFloat("moveX", inputDirection.x);
            animator.SetFloat("moveY", inputDirection.z);
            
            //Call the camera aim function
            //m_cameraController.AimCamera(true, (int)inputDirection.x);
     
        }

        else
        {
            //m_crossHair.CrossFadeAlpha(0,0.1f,false);
            Cursor.lockState = CursorLockMode.None;
            m_isAttackAiming = false;
            animator.SetLayerWeight(1,0);
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", 0);
            animator.SetBool("attackAim", m_isAttackAiming);
            //m_cameraController.AimCamera(false);
        }
    }

    //Updates the audio based on player move state
    private void UpdateAudio()
    {   
        //If we are running / walking then only play footsteps slound
        if( (m_moveState == MoveState.Run || m_moveState == MoveState.Walk) 
            && m_speed >= m_walkSpeed)
        {
            //Running
            if (m_speed > m_walkSpeed)
            {
                //footstepsAudioManager.PlayRandom();
            }
            //Walking
            else
            {
                //footstepsAudioManager.PlayRandom(0.4f, 0.8f);
            }
        }
    }
   

    //Updates the rotation of player to face in the movement direction
    private void UpdateRotation()
    {
        //If we have some input and not currently attacking / aiming, then only rotate in input direction
        // other wise preserve the rotation
        if (inputDirection != Vector3.zero && !m_isAttackAiming)            
        {
            //Calcuate the angle required to rotate based on the given direction
            float turnAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;

            //Convert the angle into a Quaternion (Data Type used by many game engines for rotation)
            //It consists of 4 dimensions: 3 imaginary numbers (x , y , z) and a real number (w) *Notations can differ
            Quaternion targetRotation = Quaternion.Euler(0 , turnAngle, 0);

            //Smoothly Interpolate from current rotation to target rotation with the given turn speed amount 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_turnSpeed * Time.deltaTime);           
        }

        //If attacking / aiming then face the crosshair always
        else if(m_isAttackAiming)
        {
            //Calcuate the angle required to rotate based on the given direction
            float turnAngle = Mathf.Atan2(m_fireRay.direction.x, m_fireRay.direction.z) * Mathf.Rad2Deg;

            //Convert the angle into a Quaternion (Data Type used by many game engines for rotation)
            //It consists of 4 dimensions: 3 imaginary numbers (x , y , z) and a real number (w) *Notations can differ
            Quaternion targetRotation = Quaternion.Euler(0 , turnAngle, 0);

            //Smoothly Interpolate from current rotation to target rotation with the given turn speed amount 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_turnSpeed * Time.deltaTime);  
        }

    }

    //Updates the L-R-F-B movement
    private void UpdateHorizontalMovement()
    {
       
        //Get the input and normalize
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical")).normalized;
   
        //We want to preserve and modify the movement.y value when there is jump input, 
        //hence we only assign individual xz components
        movement.x = inputDirection.x * m_speed;
        movement.z = inputDirection.z * m_speed;

        //With this previous code, our movement.y will always be 0
        movement = inputDirection.normalized * m_speed;


        //Camera forward (Player Forward), Camera right (Player right)
        movement = (playerCam.forward * inputDirection.z + playerCam.right * inputDirection.x) * m_speed;

        //Play the movement animation
        //This will take care of both the idle and run animations since for idle speed will 0 when there is no input
        animator.SetFloat("speed", m_speed);
       
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
                m_speedY = m_jumpHeight;

                //For constant speed while jumping
                //m_speed = m_runSpeed;

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

            //animator.SetBool("running", false);
           // animator.SetFloat("speed", 0);

            //Apply gravity so that the player falls back to the ground
            m_speedY -= GRAVITY * Time.deltaTime;
           
        }      

        movementJump.y = m_speedY;
        
    }


    //Updates current player state
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

            //Idle when no input, and player is grounded
            else
            {
                m_speed = 0; 
                m_moveState = MoveState.Idle;
                animator.SetBool("running", false);
            }


            //Check for Crawl when not attacking. Totally design design
            if(Input.GetKey(KeyCode.LeftControl) && !m_isAttackAiming)
            {
                //Crawl moving or idle?
                if(inputDirection != Vector3.zero)
                    m_speed = Mathf.MoveTowards(m_speed, m_crawlSpeed, smoothSpeed * Time.deltaTime);
                else
                    m_speed = Mathf.MoveTowards(m_speed, 0, smoothSpeed * Time.deltaTime);

                animator.SetBool("crawl", true);
            
                m_moveState = MoveState.Crawl;

            }

            else
            {
                animator.SetBool("crawl", false);
            }
   
        } 
    }
}
