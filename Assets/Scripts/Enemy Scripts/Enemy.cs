using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float enemyHealth;
    private Transform target;
    public float walkSpeed = 4f;
    public float gallopSpeed = 4f;
    public float currentSpeed = 2f;
    public float rotationSpeed = 0.2f;

    public float seenWidth;
    public float seenDepth;

    public float chaseWidth;
    public float chaseDepth;
    public bool isGalloping = false;

    private Player player;
    private PlayListCycler playlist;
    private CharacterController enemyController;
    private Animator anim;

    void Start()
    {
        setEnemyHealth(1f);
        playlist = FindObjectOfType<PlayListCycler>();
        if (playlist == null)
        {
            Debug.Log("Could not locate playlist in enemy class");
        }
        //rigidb = GetComponent<Rigidbody>();

        enemyController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        player = GameObject.FindObjectOfType<Player>();
        target = player.transform;
    }


    public void Update()
    {
        Vector3 dist = target.transform.position - transform.position;
        float absX = Mathf.Abs(dist.x);
        float absZ = Mathf.Abs(dist.z);


        if (absZ < seenDepth || absX < seenWidth) // Target player by walking
        {
            currentSpeed = walkSpeed;
            anim.SetBool("hasSeenPlayer", true);

            // Make enemy look at player
            var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            /*
            if (absZ < chaseDepth || absX < chaseWidth)
            {
                isGalloping = true;
                anim.SetBool("isGalloping", true);
                // Target player by walking
                // Move enemy towards location of player
                transform.position = Vector3.MoveTowards(transform.position, target.position, gallopSpeed * Time.fixedDeltaTime);

            }
            else
            {
                isGalloping = false;

                // Walk animation
                anim.SetFloat("speed", walkSpeed);

            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, walkSpeed * Time.fixedDeltaTime);
            anim.SetBool("isGalloping", isGalloping);
            */
        }
        else
        {
            anim.SetBool("hasSeenPlayer", false);

            //anim.SetBool("isGalloping", isGalloping);
            currentSpeed = 0f;
        }

        anim.SetFloat("speed", currentSpeed);
        
        transform.position = Vector3.MoveTowards(transform.position, target.position, currentSpeed * Time.fixedDeltaTime);

    }


    public void idle()
    {
        // Play idling animation
    }
    public void chargeAtPlayer()
    {
        // Play charging animation
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
