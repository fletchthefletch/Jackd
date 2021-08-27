using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float enemyHealth;
    private Transform target;
    public float speed = 4f;
    public float rotationSpeed = 0.2f;
    public Rigidbody rigidb;
    private Player player;
    private PlayListCycler playlist;

    void Start()
    {
        setEnemyHealth(1f);
        playlist = FindObjectOfType<PlayListCycler>();
        if (playlist == null)
        {
            Debug.Log("Could not locate playlist in enemy class");
        }
        rigidb = GetComponent<Rigidbody>();
        player = GameObject.FindObjectOfType<Player>();
        target = player.transform;
    }


    public void Update()
    {
        // Move enemy towards location of player
        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        rigidb.MovePosition(pos);

        // Make enemy look at player
        //var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        // Smoothly rotate towards the target point.
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.LookAt(target);
        //transform.Translate(Vector3.forward * 3 * Time.deltaTime);
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
