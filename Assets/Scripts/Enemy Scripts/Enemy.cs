using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 1.4f;
    [SerializeField]
    private float gallopSpeed = 2.1f;
    [SerializeField]
    private float enemyHealth;
    [SerializeField]
    private float currentSpeed = 0f;
    [SerializeField]
    private float rotationSpeed = 2f;

    [SerializeField]
    private float seenDepth = 10.0f;
    [SerializeField]
    private float chaseDepth;

    [SerializeField]
    private bool isGalloping = false;
    [SerializeField]
    private bool hasSeenPlayer = false;
    [SerializeField]
    private bool isEating = false;

    private bool stopMoving = false;
    private bool kickToggle = false;


    [SerializeField]
    private float timeUntilEnemyEats = 5f;
    private float eatTimer = 0f;
    private float sqrLen;
    private Vector3 dist;

    // Gameobjects
    private Player player;
    private Transform target;
    private PlayListCycler playlist;
    private CharacterController enemyController;
    private Animator anim;


    void Start()
    {
        setEnemyHealth(1f);

        // Retrieve game objects
        enemyController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        if (player == null)
        {
            Debug.Log("Could not locate player");
        }
        playlist = FindObjectOfType<PlayListCycler>();
        if (playlist == null)
        {
            Debug.Log("Could not locate playlist in enemy class");
        }
        target = player.transform;

        chaseDepth = seenDepth / 2f;
    }


    private void OnTriggerEnter(Collider other)
    {     
        if (other.CompareTag("Player"))
        {
            // Check the angle
            float dot = Vector3.Dot(transform.forward, 
               (target.position - transform.position).normalized);

            // Get back fan
            // Get front fan
            if (dot < -0.5)
            {
                // Kick
               // kickToggle = true;
            }
            else 
            {
             //   kickToggle = false;
            }
            //Debug.Log(dot.ToString());
            

            stopMoving = true;
            //anim.SetBool("kickToggle", kickToggle);
            //anim.SetBool("stopMoving", stopMoving);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            stopMoving = false;
            //anim.SetBool("stopMoving", stopMoving);
        }
    }

    private void rotateTowardsPlayer()
    {
        // Make enemy look at player
        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    /*
    public void Update1()
    {
        Vector3 dist = (target.transform.position - transform.position).normalized;
        float absX = Mathf.Abs(dist.x);
        float absZ = Mathf.Abs(dist.z);

        currentSpeed = 0f;
        hasSeenPlayer = false;

        if (stopMoving)
        {
            isEating = false;
            return;
        }

        if (absZ < seenDepth || absX < seenWidth) // Target player by walking
        {
            // Stop eating if the player is
            isEating = false;
            eatTimer = 0f;

            hasSeenPlayer = true;
            rotateTowardsPlayer();

            if (absZ < chaseDepth || absX < chaseWidth)
            {
                currentSpeed = gallopSpeed;
                isGalloping = true;
                // Target player by walking
                // Move enemy towards location of player
            }
            else
            {
                isGalloping = false;
                currentSpeed = walkSpeed;
            }
        }
        else
        {
            isGalloping = false;
            
            if (eatTimer >= timeUntilEnemyEats)
            {
                // Start eating
                isEating = true;
            }
            else
            {
                eatTimer += Time.deltaTime;
            }
        }

        anim.SetFloat("speed", currentSpeed);
        anim.SetBool("isGalloping", isGalloping);
        anim.SetBool("hasSeenPlayer", hasSeenPlayer);
        anim.SetBool("isHungry", isEating);

        transform.position = Vector3.MoveTowards(transform.position, target.position, currentSpeed * Time.fixedDeltaTime);
    }
    */

    public void Update()
    {
        if (stopMoving)
        {
            return;
        }

        dist = target.transform.position - transform.position;
        sqrLen = dist.sqrMagnitude;

        if (sqrLen > seenDepth * seenDepth)
        {
           // resetEverything();
            anim.SetBool("hasSeenPlayer", false);
            anim.SetBool("isGalloping", false);
            currentSpeed = 0f;

            if (eatTimer >= timeUntilEnemyEats)
            {
                // Start eating
                anim.SetBool("isHungry", true);
            }
            else
            {
                eatTimer += Time.deltaTime;
            }
            return; // We don't care about the player's position
        }
        
        // Face player
        rotateTowardsPlayer();

        if (sqrLen < seenDepth * seenDepth)
        {
            runTowardsPlayer();
            return;
        }
   }
    private void resetEverything()
    {
        walkSpeed = 1.4f;
gallopSpeed = 2.1f;
 currentSpeed = 0f;
rotationSpeed = 2f;

seenDepth = 10.0f;
 chaseDepth = seenDepth / 2f;

 isGalloping = false;
 hasSeenPlayer = false;
    isEating = false;
    }

    private void walkTowardsPlayer()
    {
        // Start walking towards player
        anim.SetBool("hasSeenPlayer", true);
        anim.SetBool("isHungry", false);
        anim.SetBool("isGalloping", false);
        currentSpeed = walkSpeed;
        eatTimer = 0f;
        updatePosition();
    }
    private void runTowardsPlayer()
    {
        // Start walking towards player
        anim.SetBool("hasSeenPlayer", true);
        anim.SetBool("isGalloping", true);
        anim.SetBool("isHungry", false);
        currentSpeed = gallopSpeed;
        eatTimer = 0f;
        updatePosition();
    }

    private void updatePosition()
    {
        // Move enemy
        transform.position = Vector3.MoveTowards(transform.position, target.position, currentSpeed * Time.fixedDeltaTime);
    }

    public void setEnemyHealth(float val)
    {
        // Update health var
        enemyHealth = val;
        return;
    }
    public float getEnemyHealth()
    {
        return enemyHealth;
    }
    public bool takeDamage(float damageAmount)
    {
        float res = enemyHealth - damageAmount;
        if (res >= 0f)
        {
            setEnemyHealth(res);
            return true;
        }
        else
        {
            // Enemy is dead! Should destroy this enemy in the enemy manager
            setEnemyHealth(0f);
            return false;

        }
    }
    public void heal(float healAmount)
    {
        float res = enemyHealth + healAmount;
        if (res <= 1f)
        {
            setEnemyHealth(res);
            return;
        }
        else
        {
            setEnemyHealth(1f);
            return;

        }
    }
}
