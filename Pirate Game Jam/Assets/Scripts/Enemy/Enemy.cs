using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHealth, IItemPicker
{
    protected Rigidbody2D rb;
    protected Animator anim;
    SpriteRenderer sr;

    const float MAX_CAMERA_HEIGHT = 17f;

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
    float fireRateMultiplier = 1;

    Collider2D[] colliders;

    //the type of weapon the enemy has
    protected EnemyWeapon weapon;

    protected PlayerMovement player;

    public float Health { get; set; }
    public float MaxHealth { get; set; } = 150;

    //fields for the flash
    float intervalTimer;
    ShortColorFlash shortColorFlash;
    LongColorFlash longColorFlash;

    //statBoostTimer
    float damageBoostTime;
    float damageBoostTimer;
    float fireRateBoostTime;
    float fireRateBoostTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        //weapon = GetComponent<EnemyWeapon>();
        weapon = GetComponentInChildren<EnemyWeapon>();
        Health = MaxHealth;
        longColorFlash = new();

        if (weapon == null)
            return;
        shootingTimer = weapon.shootingTimer;
        lastShot = weapon.lastShot;
    }

    // doing it on start and not on awake so to avoid racing conflicts
    void Start() => GameManager.Instance.Enemies.Add(this);

    void OnDisable() => GameManager.Instance.Enemies.Remove(this);

    void Update()
    {
        // Plays Idle animation when not moving in x-axis, otherwise plays Walk animation
        if (MathF.Abs(rb.linearVelocityX) < .25f)
            anim.Play("Idle");
        else
            anim.Play("Walk");

        if(shootingTimer > lastShot)
            lastShot += Time.deltaTime;

        CheckStatBoost();
        ColorFlash();

        if (player != null)
        {
            Vector3 flatPlayerPos = player.transform.position;
            flatPlayerPos.y = transform.position.y;
            if ((flatPlayerPos - transform.position).magnitude > lookDistance / 2)
            {
                player = null;
                weapon.Reset();
                if(weapon != null && weapon is EnemyAssaultRifle assaultRifle)
                    assaultRifle.ResetTimers();
                return;
            }

            FaceDirection(player.transform.position - transform.position);

            if(weapon == null)
                return;
            
            //taking aim
            Vector2 shootDir = (player.transform.position - transform.position).normalized;
            Vector3 upwards = Vector3.Cross(Vector3.forward, shootDir);
            Vector3 forward = Vector3.forward;

            if(transform.rotation.y % 360 != 0)
            {
                forward = -forward;
                upwards = -upwards;
            }

            Quaternion aimRot = Quaternion.LookRotation(forward, upwards);
            weapon.SpriteRotation(aimRot);

            if(lastShot >= shootingTimer * fireRateMultiplier)
            {
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
                        Shoot(shootDir, aimRot);
                        assaultRifle.fireRateTimer = 0;
                    }
                    else
                        assaultRifle.fireRateTimer += Time.deltaTime;
                    
                    assaultRifle.fireTimer += Time.deltaTime;
                }
                else
                {
                    Shoot(shootDir, aimRot);
                    lastShot = 0;
                }
            }
            else if (lastShot >= 0.1f && lastShot < shootingTimer)
            {
                weapon.Idle();
            }
        }
    }

    void CheckStatBoost()
    {
        if(damageBoostTime != 0)
        {
            if (damageBoostTimer >= damageBoostTime)
            {
                weapon.damageMultiplier = 1;
                damageBoostTime = 0;
                damageBoostTimer = 0;
            }
            else
                damageBoostTimer+= Time.deltaTime;
        }

        if(fireRateBoostTime != 0)
        {
            if (fireRateBoostTimer >= fireRateBoostTime)
            {
                fireRateMultiplier = 1;
                fireRateBoostTime = 0;
                fireRateBoostTimer = 0;
            }
            else
                fireRateBoostTimer+= Time.deltaTime;
        }
    }

    void ColorFlash()
    {
        if(shortColorFlash.duration > 0)
        {
            sr.color = shortColorFlash.Color;
            shortColorFlash.duration-= Time.deltaTime;
            if(shortColorFlash.duration <= 0)
            {
                sr.color = Color.white;
                shortColorFlash = default;
            }
            else
            {
                if(!longColorFlash.IsEmpty())
                    longColorFlash.ReduceDurations(Time.deltaTime);
                return;
            }
        }

        if(!longColorFlash.IsEmpty())
        {
            if(intervalTimer <= longColorFlash.Interval)
                intervalTimer+= Time.deltaTime;
            else
                intervalTimer = 0;

            sr.color = longColorFlash.GetColor(intervalTimer);
            longColorFlash.ReduceDurations(Time.deltaTime);

            return;
        }

        sr.color = Color.white;
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
        float enemyToPlayerFlatDist = enemyToPlayerFlat.magnitude;
        Vector2 dir = enemyToPlayerFlat.normalized;

        FaceDirection(dir);
        if(enemyToPlayerFlatDist < (maxDistance + maxDistanceOffset) / 2 && enemyToPlayerFlatDist > (maxDistance - maxDistanceOffset) / 2)
        {
            rb.linearVelocityX = 0;
            return;
        }
        
        if(enemyToPlayerFlatDist < (maxDistance - maxDistanceOffset) / 2)
            dir = -dir;
        
        AddForce(dir, 3);
    }

    protected void LookForPlayer()
    {
        //getting a flat position of this enemy
        Vector2 enemyFlatPos = transform.position;
        enemyFlatPos.y = 0;
        colliders = Physics2D.OverlapBoxAll(enemyFlatPos, new(lookDistance, MAX_CAMERA_HEIGHT), 0);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out player))
            {
                FaceDirection((player.transform.position - transform.position).normalized);
                break;
            }
        }
    }

    public void FaceDirection(Vector2 dir)
    {
        if (dir.x < 0) // Left Direction
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (dir.x > 0) // Right Direction
            transform.rotation = Quaternion.identity;
    }

    protected void AddForce(Vector2 dir, float accelRate)
    {
        Vector2 velocity = dir * speed;
        Vector2 velocityDiff = velocity - rb.linearVelocity;
        if(dir == Vector2.zero)
            accelRate = 6;
        Vector2 force = velocityDiff * accelRate;
        rb.AddForce(force);
    }

    protected virtual void Shoot(Vector2 dir, Quaternion aimRot) => weapon.Shoot(player, aimRot);

    public void Heal(float healAmount)
    {
        Health+= healAmount;
        if(Health > MaxHealth)
            Health = MaxHealth;
        shortColorFlash = new(Color.green);
        Debug.Log(gameObject.name + " Health: " + Health);
    }

    public void Damage(float damageAmount)
    {
        Health-= damageAmount;
        shortColorFlash = new(Color.red);
        Debug.Log(gameObject.name + " Health: " + Health);
        if(Health <= 0)
            Die();
    }

    //TODO
    public void Die()
    {
        ObjectivesManager.Instance.checkingObjectives();
        Destroy(gameObject);
    }

    public void PickItem(PickableItem item) => item.Effect(this);

    public void DamageBoost(float DamageBoostAmount, float duration)
    {
        weapon.damageMultiplier = DamageBoostAmount;
        damageBoostTime+= duration;
        longColorFlash.AddColor(new(0.2f, 0.6f, 1), duration);
    }

    public void FireRateBoost(float FireRateBoostAmount, float duration)
    {
        fireRateMultiplier = FireRateBoostAmount;
        fireRateBoostTime+= duration;
        longColorFlash.AddColor(Color.yellow, duration);
    }

    void OnDrawGizmos()
    {
        if (debugMode)
        {
            Vector2 flatPos = new(transform.position.x, 0);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(flatPos, .25f);
            Gizmos.DrawWireCube(flatPos, new(lookDistance, MAX_CAMERA_HEIGHT));
    
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(flatPos, new(maxDistance, MAX_CAMERA_HEIGHT));
    
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(flatPos, new(maxDistance - maxDistanceOffset, MAX_CAMERA_HEIGHT));
            Gizmos.DrawWireCube(flatPos, new(maxDistance + maxDistanceOffset, MAX_CAMERA_HEIGHT));
            
            Gizmos.color = Color.red;
            foreach (Vector2 waypoint in waypoints)
                Gizmos.DrawSphere(waypoint, .25f);
        }
    }
}
