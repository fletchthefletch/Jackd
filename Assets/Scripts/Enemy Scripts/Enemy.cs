using System.Collections;
using UnityEngine;
// This class manages an enemy instance

public class Enemy : MonoBehaviour
{
    private float gallopSpeed;
    private float enemyHealth;
    private float currentSpeed = 0f;
    private float rotationSpeed;
    private float seenDepth;
    private float chaseDepth;
    private float enemyKickDamage = 0.25f;
    private float enemyHeadButtDamage = 0.15f;
    private float displayAfterDeathTime = 5f;
    private float timeUntilEnemyEats = 5f;
    private float eatTimer = 0f;
    private float sqrLen = 1000000f;
    private Vector3 dist;
    private bool isAlive;
    public int id;
    private bool oneMoo = true;
    private float hitRange = 2.3f;
    private int enemyScoreValue = 50;
    private bool gameIsPaused = false;
    private bool enabledEnem = true;

    // Gameobjects
    private Player player;
    private Transform target;
    private PlayListCycler playlist;
    private CharacterController enemyController;
    private Animator anim;
    private EnemyManager manager;
    private PauseMenu menu;

    // Enemy minimap icon
    [SerializeField]
    private GameObject icon;
    private SpriteRenderer iconRenderer;
    [SerializeField]
    private Color healthBadColor;
    [SerializeField]
    private Color healthAverageColor; 
    

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
        iconRenderer = icon.GetComponent<SpriteRenderer>();

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

    public void setEnabled(bool enable)
    {
        this.enabledEnem = enable;

        //Deletecode
        if (enabledEnem)
        {
            iconRenderer.color = Color.blue;
        }
        else
        {
            iconRenderer.color = Color.red;
        }
        
    }
    public bool getEnabled()
    {
        return enabledEnem;
    }
    public void setEnemy(int enemyType)
    {
        switch (enemyType)
        {
            case 0:
                // Cow
                enemyHeadButtDamage = 0.15f;
                enemyKickDamage = 0.25f;
                rotationSpeed = 1f;
                gallopSpeed = 1.2f;
                seenDepth = 7f;
                chaseDepth = 3f; 
                setEnemyHealth(1f);
                break;
            case 1:
                // Bull
                enemyHeadButtDamage = 0.35f;
                enemyKickDamage = 0.3f;
                rotationSpeed = 1.5f;
                gallopSpeed = 1.6f;
                seenDepth = 7f;
                chaseDepth = 3f;
                setEnemyHealth(2f);
                break;
        }
    }
  
    private IEnumerator enemyDeath()
    {
        anim.SetBool("isAlive", false);

        // Delay
        yield return new WaitForSecondsRealtime(displayAfterDeathTime);

        // Destroy enemy
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
            // Enemy should stop moving
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
            // Player is behind enemy --> Kick
            anim.SetBool("isKicking", true);
        }
        else
        {
            // Player is in front of enemy --> headbutt
            anim.SetBool("isKicking", false);
        }
    }

    public void headButtHit()
    {
        // Check distance
        if (sqrLen > hitRange * hitRange)
        {
            return;
        }

        // Check the angle
        float dot = Vector3.Dot(transform.forward, (target.position - transform.position).normalized);
        if (dot < -0.3)
        {
            // Player has been kicked!
            // Kick range
            playlist.playInteractionSound("stab", true);
            player.takeDamage(enemyKickDamage); // 0.25f
        }
        else if (dot > 0.75)
        {
            // Player has been headbutted!
            // Headbutt range
            playlist.playInteractionSound("stab", true);
            player.takeDamage(enemyHeadButtDamage); // 0.15f
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

    public float checkDistanceToPlayer()
    {
        return sqrLen;    
    }
    private void getDistanceToPlayer()
    {
        dist = target.transform.position - transform.position;
        sqrLen = dist.sqrMagnitude;
    }

    public void setGamePaused(bool val)
    {
        this.gameIsPaused = val;
        if (val)
        {
            // Freeze current animation 
            anim.speed = 0;
        }
        else
        {
            // Unfreeze animation
            anim.speed = 1;
        }
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
        if (gameIsPaused)
        {
            // Game is paused
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

        getDistanceToPlayer();
        
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
            
            if (!enabledEnem)
            {
                if (anim.GetBool("isGalloping"))
                {
                    anim.SetBool("isGalloping", false);
                }
                if (anim.GetBool("hasSeenPlayer"))
                {
                    anim.SetBool("hasSeenPlayer", false);
                }
                return;
            }


            // Enemy can see player
            rotateTowardsPlayer();
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

    private void setEnemyHealth(float val)
    {
        // Update health var
        enemyHealth = val;
        return;
    }
    private float getEnemyHealth()
    {
        return enemyHealth;
    }
    public bool takeDamage(float damageAmount)
    {
        playlist.playInteractionSound("thunk", true);
        float res = enemyHealth - damageAmount;
        if (res > 0f)
        {
            setEnemyHealth(res);
            if (res <= 0.25)
            {
                iconRenderer.color = healthBadColor;
            }
            else if (res <= 75)
            {
                iconRenderer.color = healthAverageColor;
            }
            return true;
        }
        else
        {
            // Enemy is dead! Should destroy this enemy in the enemy manager
            player.setPlayerScore(enemyScoreValue); ;
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
