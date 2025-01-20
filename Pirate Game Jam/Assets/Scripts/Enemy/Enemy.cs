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
    
    float shootingTimer = 2;
    float  lastShot = 2;
    Collider2D[] colliders;

    //the type of weapon the enemy has
    protected EnemyWeapon weapon;

    protected PlayerMovement player;

    public float Health { get; set; } = 100;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //weapon = GetComponent<EnemyWeapon>();
        weapon = GetComponentInChildren<EnemyWeapon>();

        if (weapon == null)
            return;
        shootingTimer = weapon.shootingTimer;
    }

    // doing it on start and not on awake so that we avoid racing conflicts
    void Start() => GameManager.Instance.Enemies.Add(this);

    void OnDisable() => GameManager.Instance.Enemies.Remove(this);

    void Update()
    {
        // Plays Idle animation when not moving in x-axis, otherwise plays Walk animation
        if (rb.linearVelocityX == 0f)
        {
            anim.Play("Idle");
        }
        else
        {
            anim.Play("Walk");
        }

        if(shootingTimer > lastShot)
            lastShot += Time.deltaTime;

        if (player != null)
        {
            if ((player.transform.position - transform.position).magnitude > lookDistance)
            {
                player = null;
                if(weapon != null && weapon is EnemyAssaultRifle assaultRifle)
                    assaultRifle.ResetTimers();
                return;
            }

            if(weapon == null)
                return;
            
            if(lastShot >= shootingTimer)
            {   
                Vector2 shootDir = (player.transform.position - transform.position).normalized;

                //doing it custom for assault rifle since it is the only automatic weapon
                if(weapon is EnemyAssaultRifle assaultRifle)
                {
                    if(assaultRifle.FireAmount <= assaultRifle.fireTimer)
                    {
                        assaultRifle.fireTimer = 0;
                        lastShot = 0;
                        return;
                    }

                    if(assaultRifle.FireRate <= assaultRifle.fireRateTimer)
                    {
                        Shoot(shootDir);
                        assaultRifle.fireRateTimer = 0;
                    }
                    else
                        assaultRifle.fireRateTimer += Time.deltaTime;
                    
                    assaultRifle.fireTimer += Time.deltaTime;
                }
                else
                {
                    Shoot(shootDir);
                    lastShot = 0;
                }
            }
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
        
        FaceDirection(enemyToPoint);
        AddForce(enemyToPoint.normalized, 3);
    }

    protected virtual void ChasePlayer()
    {
        // getting a copy vector of the player and making that vector y equals to the this enemy
        Vector3 playerFlat = player.transform.position;
        playerFlat.y = transform.position.y;
        Vector2 enemyToPlayerFlat = playerFlat - transform.position;
        Vector2 dir = enemyToPlayerFlat.normalized;

        FaceDirection(dir);

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
        Vector2 velocity = dir * speed;
        Vector2 velocityDiff = velocity - rb.linearVelocity;
        Vector2 force = velocityDiff * accelRate;
        rb.AddForce(force);
    }

    protected virtual void Shoot(Vector2 dir)
    {
        Vector3 upwards = Vector3.Cross(Vector3.forward, dir);
        Quaternion aimRot = Quaternion.LookRotation(Vector3.forward, upwards);
        weapon.Shoot(dir, aimRot);
    }

    public void Damage(float damageAmount)
    {
        Health-= damageAmount;
        Debug.Log(Health);
        if(Health <= 0)
            Die();
    }

    //TODO
    public void Die()
    {
        
    }

    public void FaceDirection(Vector2 dir)
    {
        if (dir.x < 0) // Left Direction
        {
            transform.localScale = new Vector2(-1, 1);
        }
        if (dir.x > 0) // Right Direction
        {
            transform.localScale = new Vector2(1, 1);
        }
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
