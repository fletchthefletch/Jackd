using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float gallopSpeed = 1.2f; // 1.3f
    private float enemyHealth;
    private float currentSpeed = 0f;
    private float rotationSpeed = 2f;
    private float seenDepth = 10.0f;
    private float chaseDepth = 3f;

    [SerializeField]
    private bool isGalloping = false;
    [SerializeField]
    private bool hasSeenPlayer = false;
    [SerializeField]
    private bool isEating = false;

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

    private bool oneMoo = true;




    void Start()
    {

        isAlive = true;
        this.id = 0;
        player = FindObjectOfType<Player>();
        setEnemyHealth(1f);
        target = player.transform;

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
    public void setChaseDepth(int enemyType)
    {
        switch (enemyType)
        {
            case 0:
                // Cow
                chaseDepth = 3f;
                break;
            case 1:
                // Bull
                chaseDepth = 4f;
                break;
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
            // Cow should stop moving
            anim.SetBool("stopMoving", true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!anim.GetBool("stopMoving"))
        {
            return;
        }
        // Check the angle
        float dot = Vector3.Dot(transform.forward, (target.position - transform.position).normalized);

        if (dot < -0.5)
        {
            // Player is behind cow --> Kick
            anim.SetBool("isKicking", true);
        }
        else
        {
            // Player is in front of cow --> headbutt
            anim.SetBool("isKicking", false);
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
            //anim.SetBool("stopMoving", false);
            anim.SetBool("isKicking", false);
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
            // Start death animation --> enemy is dead
            if (isAlive)
            {
                StartCoroutine(enemyDeath());
                isAlive = false;
            }
        }

        dist = target.transform.position - transform.position;
        sqrLen = dist.sqrMagnitude;

        if (anim.GetBool("stopMoving"))
        {
            if (oneMoo)
            {
                playlist.playInteractionSound("moo", true);
                oneMoo = false;
            }
            if (sqrLen > chaseDepth * chaseDepth)
            {
                anim.SetBool("stopMoving", false);
            }
            else
            {
                return;
            }          
        }

        if (sqrLen > seenDepth * seenDepth)
        {
            // Enemy can't see player
            anim.SetBool("hasSeenPlayer", false);
            anim.SetBool("isGalloping", false);
            oneMoo = true;
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
        else
        {
            // Enemy can see player
            // Face player
            rotateTowardsPlayer();
            // Start walking towards player
            anim.SetBool("hasSeenPlayer", true);
            anim.SetBool("isHungry", false);
            eatTimer = 0f;

            // Move player
            anim.SetBool("isGalloping", true);
            currentSpeed = gallopSpeed;
            updatePosition();
            return;
        }
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
        playlist.playInteractionSound("stab", true);
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
