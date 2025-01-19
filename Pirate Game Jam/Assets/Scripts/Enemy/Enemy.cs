using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHealth
{
    protected Rigidbody2D rb;
    protected Animator anim;

    [SerializeField] List<Vector2> waypoints;
    int waypointIndex;
    //the time to spend between each waypoint
    [SerializeField] float waypointTime = 1;
    float lastWaypointTime;

    [SerializeField] bool debugMode = false;
    [SerializeField] protected float lookDistance = 5;
    [SerializeField] protected float maxDistance = 3;
    [SerializeField] protected float speed = 5;
    protected float maxDistanceOffset = .75f;
    
    [SerializeField] protected float shootingCooldown = 2;
    float  lastShot = 2;
    Collider2D[] colliders;

    protected PlayerMovement player;

    public float Health { get; set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(shootingCooldown > lastShot)
            lastShot += Time.deltaTime;

        if (player != null)
        {
            Vector2 shootDir = (player.transform.position - transform.position).normalized;
            if(lastShot >= shootingCooldown)
                Shoot(shootDir);

            if ((player.transform.position - transform.position).magnitude > lookDistance)
                player = null;
        }
    }

    protected void Patrol()
    {
        if(waypoints.Count == 0)
            return;
        
        Vector3 waypoint = waypoints[waypointIndex];
        Vector2 enemyToPoint = waypoint - transform.position;

        if(enemyToPoint.magnitude < .1f)
        {
            lastWaypointTime += Time.fixedDeltaTime;
            if(lastWaypointTime < waypointTime)
                return;

            lastWaypointTime = 0;
            waypointIndex = waypointIndex != waypoints.Count - 1 ? waypointIndex + 1 : 0;
            waypoint = waypoints[waypointIndex];
            enemyToPoint = waypoint - transform.position;
        }

        AddForce(enemyToPoint.normalized, 3);
    }

    protected virtual void ChasePlayer()
    {
        // getting a copy vector of the player and making that vector y equals to the this enemy
        Vector3 playerFlat = player.transform.position;
        playerFlat.y = transform.position.y;
        Vector2 enemyToPlayerFlat = playerFlat - transform.position;
        Vector2 dir = enemyToPlayerFlat.normalized;

        // getting the non flat distance to calculate the actual distance
        float enemyToPlayer = (player.transform.position - transform.position).magnitude;
        if(enemyToPlayer < maxDistance + maxDistanceOffset && enemyToPlayer > maxDistance - maxDistanceOffset)
        {
            rb.linearVelocityX = 0;
            return;
        }
        
        if(enemyToPlayer < maxDistance - maxDistanceOffset)
            dir = -dir;
        
        AddForce(dir, 3);
    }

    protected void LookForPlayer()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, lookDistance);

        foreach (Collider2D collider in colliders)
            if(collider.TryGetComponent(out player))
                break;
    }

    protected void AddForce(Vector2 dir, float accelRate)
    {
        if (rb.linearVelocityX == 0f) // Plays Idle animation when not moving
        {
            anim.Play("Idle");
        }
        else if (dir.x != 0) // Plays Walk animation and flips the sprite depending on direction it's moving
        {
            anim.Play("Walk");
            if (dir.x < 0) // Left Direction
            {
                transform.localScale = new Vector2(-1, 1);
            }
            if (dir.x > 0) // Right Direction
            {
                transform.localScale = new Vector2(1, 1);
            }
        }
        

        Vector2 velocity = dir * speed;
        Vector2 velocityDiff = velocity - rb.linearVelocity;
        Vector2 force = velocityDiff * accelRate;
        rb.AddForce(force);
    }

    // TODO
    protected virtual void Shoot(Vector2 dir)
    {
        lastShot = 0;
        Debug.Log("Shoot: " + dir);
    }

    public void Damage(int damageAmount)
    {
        Health-= damageAmount;
        if(Health <= 0)
            Die();
    }

    //TODO
    public void Die()
    {
        
    }

    void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, lookDistance);
    
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxDistance);
    
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, maxDistance + maxDistanceOffset);
            Gizmos.DrawWireSphere(transform.position, maxDistance - maxDistanceOffset);
            
            Gizmos.color = Color.red;
            foreach (Vector2 waypoint in waypoints)
                Gizmos.DrawSphere(waypoint, .25f);
        }
    }
}
