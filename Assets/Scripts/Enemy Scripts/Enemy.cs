using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float walkSpeed = 1f; // 1.4f
    private float gallopSpeed = 1f; // 1.9f
    private float enemyHealth;
    private float currentSpeed = 0f;
    private float rotationSpeed = 2f;
    private float seenDepth = 10.0f;
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
    private float displayAfterDeathTime = 5f;

    [SerializeField]
    private float timeUntilEnemyEats = 5f;
    private float eatTimer = 0f;
    private float sqrLen;
    private Vector3 dist;
    [SerializeField]
    private bool isAlive;

    // Gameobjects
    private Player player;
    private Transform target;
    private PlayListCycler playlist;
    private CharacterController enemyController;
    private Animator anim;
    private EnemyManager manager;
    public int id;
    //Deletecode
    private bool oneTime = false;

    void Start()
    {

        isAlive = true;
        this.id = 0;
        player = FindObjectOfType<Player>();
        setEnemyHealth(1f);
        target = player.transform;

        chaseDepth = seenDepth / 2f;
        // Retrieve game objects
        enemyController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        manager = FindObjectOfType<EnemyManager>();
        if (player == null)
        {
            Debug.Log("Could not locate player");
        }
        playlist = FindObjectOfType<PlayListCycler>();
        if (playlist == null)
        {
            Debug.Log("Could not locate playlist in enemy class");
        }        
    }
 
    private IEnumerator enemyDeath()
    {
        anim.SetBool("isAlive", false);

        // Delay
        yield return new WaitForSecondsRealtime(displayAfterDeathTime);

        // Destroy cow
        Destroy(this.transform.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAlive)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            anim.SetBool("stopMoving", true);
            // Check the angle
            float dot = Vector3.Dot(transform.forward, (target.position - transform.position).normalized);

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
            

            //anim.SetBool("kickToggle", kickToggle);
            //anim.SetBool("stopMoving", stopMoving);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isAlive)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            anim.SetBool("stopMoving", false);
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
        if (id != 0)
        {
            return;
        }
        if (!isAlive)
        {
            return;
        }

        if (enemyHealth <= 0f)
        {
            if (isAlive)
            {
                StartCoroutine(enemyDeath());
                isAlive = false;
            }
        }

        if (anim.GetBool("stopMoving"))
        {
            return;
        }

        if (target == null)
        {
            Debug.Log("target");
        }
        dist = target.transform.position - transform.position;
        sqrLen = dist.sqrMagnitude;

        if (sqrLen > seenDepth * seenDepth)
        {
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
