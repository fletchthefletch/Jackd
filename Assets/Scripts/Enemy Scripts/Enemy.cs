using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 4f;
    [SerializeField]
    private float gallopSpeed = 4f;
    [SerializeField]
    private float enemyHealth;
    [SerializeField]
    private float currentSpeed = 2f;
    [SerializeField]
    private float rotationSpeed = 0.2f;
    private float seenWidth = 5f;
    private float seenDepth = 5f;
    private float chaseWidth = 2f;
    private float chaseDepth = 2f;
    private bool isGalloping = false;
    private bool hasSeenPlayer = false;
    private bool isEating = false;

    private bool stopMoving = false;


    [SerializeField]
    private float timeUntilEnemyEats = 3f;
    private float eatTimer = 0f;

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
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Determine where on the enemy the collision occurred  
        Debug.Log("Exists");
    }
    private void OnCollisionStay(Collision collision)
    {
        // Determine where on the enemy the collision occurred  
        Debug.Log("Exists");
    }
    private void OnCollisionExit(Collision collision)
    {
        // Determine where on the enemy the collision occurred  
    }
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            stopMoving = true;
            anim.SetBool("stopMoving", stopMoving);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stopMoving = false;
            anim.SetBool("stopMoving", stopMoving);
        }
    }



    private void rotateTowardsPlayer()
    {
        // Make enemy look at player
        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void Update()
    {
        Vector3 dist = target.transform.position - transform.position;
        float absX = Mathf.Abs(dist.x);
        float absZ = Mathf.Abs(dist.z);

        if (stopMoving)
        {
            return;
        }

        if (absZ < seenDepth || absX < seenWidth) // Target player by walking
        {
            // Stop eating if the player is
            isEating = false;
            eatTimer = 0f;

            currentSpeed = walkSpeed;
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
            }
        }
        else
        {
            hasSeenPlayer = false;
            currentSpeed = 0f;
            
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
