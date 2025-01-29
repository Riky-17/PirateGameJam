using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Enemy : ColorFlashObject, IHealth, IItemPicker
{
    [SerializeField] HPBar healthBarSlider;

    // protected Rigidbody2D rb;
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
    float lastShot = 2;
    float fireRateMultiplier = 1;

    Collider2D[] colliders;

    //the type of weapon the enemy has
    protected EnemyWeapon weapon;

    protected PlayerMovement player;

    public float Health { get; set; }
    public float MaxHealth { get; set; } = 100;

    //statBoostTimer
    float damageBoostTime;
    float damageBoostTimer;
    float fireRateBoostTime;
    float fireRateBoostTimer;

    //throwing item fields
    bool isThrowingItem = false;
    PickableItem itemToThrow;
    float rotatingSpeed = 2;
    float throwingTimer;
    Quaternion initialRot;
    Vector2 throwDir;
    int throwTurns = 1;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        weapon = GetComponentInChildren<EnemyWeapon>();

        Health = MaxHealth;
        healthBarSlider.UpdateHealthBar(Health, MaxHealth);

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

        CheckStatBoost();
        ColorFlash();

        if (isThrowingItem)
        {
            if(throwingTimer >= 1)
            {
                transform.rotation = initialRot;
                ThrowItem(itemToThrow);
                throwingTimer = 0;
                isThrowingItem = false;
            }
            else
            {
                throwingTimer+= Time.deltaTime * rotatingSpeed;
                Rotate();
                return;
            }
        }

        if(shootingTimer > lastShot)
            lastShot += Time.deltaTime;


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
                weapon.Idle();
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
        AddForce(enemyToPoint.normalized, speed, 3);
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
        
        AddForce(dir, speed, 3);
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

    protected virtual void Shoot(Vector2 dir, Quaternion aimRot) => weapon.Shoot(player, aimRot);

    public void Heal(float healAmount)
    {
        Health+= healAmount;
        if(Health > MaxHealth)
            Health = MaxHealth;
        healthBarSlider.UpdateHealthBar(Health, MaxHealth);
        shortColorFlash = new(Color.green);
        Debug.Log(gameObject.name + " Health: " + Health);
    }

    public void Damage(float damageAmount)
    {
        Health-= damageAmount;
        healthBarSlider.UpdateHealthBar(Health, MaxHealth);
        shortColorFlash = new(Color.red);
        Debug.Log(gameObject.name + " Health: " + Health);
        if(Health <= 0)
            Die();
    }

    public virtual void Die()
    {
        ObjectivesManager.Instance.killEnemy();
        BalancedSliderController.Instance.IncreasingSliderValue(50);
        Destroy(gameObject);
    }

    protected override void UpdateColor(Color color) => sr.color = color;

    void ThrowingInit(PickableItem statBoostItem)
    {
        if (!isThrowingItem)
        {
            isThrowingItem = true;
            float x = statBoostItem.transform.position.x - transform.position.x;
            if(x == 0)
                x = 1;

            throwDir = new(x, 0);
            throwDir = throwDir.normalized;
            throwDir += Vector2.up;
            throwDir = throwDir.normalized;
            itemToThrow = statBoostItem;
            statBoostItem.DisableItem();
            initialRot = transform.rotation;
        }
    }

    void Rotate()
    {
        float radAngle = throwTurns * (Mathf.PI * 2);
        float z = Mathf.Cos(radAngle * throwingTimer);
        float x = Mathf.Sin(radAngle * throwingTimer);

        Vector3 forward = new(x, 0, z);
        Quaternion rot = initialRot * Quaternion.LookRotation(forward, transform.up);
        transform.rotation = rot;
    }

    void ThrowItem(PickableItem item)
    {
        PickableItem itemToThrow = Instantiate(item, (Vector2)transform.position + throwDir * 2, Quaternion.identity);
        itemToThrow.ThrowItem(throwDir);
        Destroy(item.gameObject);
    }

    public void PickItem(PickableItem item)
    {
        switch(item)
        {
            case HealthItem:
                if(Health == MaxHealth)
                {
                    if(!isThrowingItem)
                        ThrowingInit(item);
                }
                else
                    item.Effect(this);
            break;
            case ReloadBoostItem:
                if(!isThrowingItem)
                    ThrowingInit(item);
            break;
            case BulletAmountBoostItem:
                if(!isThrowingItem)
                    ThrowingInit(item);
            break;
            default:
                item.Effect(this);
            break;
        }
    }

    public void DamageBoost(float DamageBoostAmount, float duration)
    {
        weapon.damageMultiplier = DamageBoostAmount;
        damageBoostTime+= duration;
        LongColorFlash.AddColor(new(0.2f, 0.6f, 1), duration);
    }

    public void FireRateBoost(float FireRateBoostAmount, float duration)
    {
        fireRateMultiplier = FireRateBoostAmount;
        fireRateBoostTime+= duration;
        LongColorFlash.AddColor(Color.yellow, duration);
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
