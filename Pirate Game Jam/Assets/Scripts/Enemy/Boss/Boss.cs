using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour, IHealth, IItemPicker
{
    const float MAX_CAMERA_WIDTH = 30 / 2;
    const float MAX_CAMERA_HEIGHT = 17 / 2;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    float maxHealth = 500;
    public float Health { get => health; set => health = value; }
    float health;

    public float DamageMultiplier => damageMultiplier;
    float damageMultiplier = 1;

    protected Rigidbody2D rb;

    public Vector2 CenterPoint => centerPoint;
    [SerializeField] Vector2 centerPoint;
    [SerializeField] protected Transform shootingPoint;

    protected PlayerMovement player;

    protected List<BossAttack> attacks;
    BossAttack currentAttack;

    bool isIdling = true;

    float idleTime = 2;
    float idleTimer;

    Vector2 moveDir;
    [SerializeField] float speed = 5;

    float walkTime = 3;
    float walkTimer;

    float waitTime = 1;
    float waitTimer;
    
    protected virtual void Awake()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        //doing it on Awake right now for testing
        Collider2D[] colliders = Physics2D.OverlapBoxAll(centerPoint, new(30, 17), 0);

        foreach (Collider2D collider in colliders)
        {
            if(collider.gameObject.TryGetComponent(out player))
                break;
        }

        InitBoss();
    }

    void Update()
    {
        if (isIdling)
        {
            TakeAim();
            if (idleTimer >= idleTime)
            {
                isIdling = false;
                PickAttack();
            }
            else
                idleTimer += Time.deltaTime;
        }

        if(currentAttack != null)
        {
            if (!currentAttack.IsAttackDone)
            {
                currentAttack.Attack();
            }
            else
            {
                if(moveDir == Vector2.zero)
                    PickDirection();
                currentAttack = null;
                isIdling = true;
                idleTimer = 0;
            }
        }
    }

    void FixedUpdate()
    {
        if(moveDir == Vector2.zero)
            return;
        
        if(walkTimer < walkTime)
        {
            walkTimer+= Time.deltaTime;
            Walk();
        }
        else
        {
            if(waitTimer >= waitTime)
            {
                waitTimer = 0;
                walkTimer = 0;
                PickDirection();
            }
            else
                waitTimer+= Time.deltaTime;
        }
    }

    public void Walk()
    {
        if(walkTimer < walkTime)
            walkTimer += Time.fixedDeltaTime;

        if((transform.position.x <= centerPoint.x - MAX_CAMERA_WIDTH && moveDir.x < 0) || (transform.position.x >= centerPoint.x + MAX_CAMERA_WIDTH && moveDir.x > 0))
        {
            rb.linearVelocityX = 0;
            walkTimer = walkTime;
        }
        
        AddForce(moveDir, speed, 3);
    }

    public void AddForce(Vector2 dir, float speed, float accel)
    {
        Vector2 velocity = dir * speed;
        Vector2 velocityDiff = velocity - rb.linearVelocity;
        Vector2 force = velocityDiff * accel;
        rb.AddForce(force);
    }

    public void Heal(float healAmount)
    {
        health+= healAmount;
        if (health > maxHealth)
            health = maxHealth;
        Debug.Log(gameObject.name + "Health: " + health);
    }

    public void Damage(float damageAmount)
    {
        health-= damageAmount;
        Debug.Log(gameObject.name + "Health: " + health);
        if(health <= 0)
            Die();
    }

    //TODO
    public void Die()
    {
        Destroy(gameObject);
    }

    public void PickItem(PickableItem item) => item.Effect(this);

    protected abstract void InitBoss();

    void PickAttack()
    {
        currentAttack = attacks[Random.Range(0, attacks.Count)];
        Debug.Log(currentAttack);
        currentAttack.InitAttack();
    }

    public virtual void RotateGun(Quaternion rotation) => shootingPoint.rotation = rotation;

    public virtual void TakeAim()
    {
        Vector2 shootDir = (player.transform.position - transform.position).normalized;
        Vector3 upwards = Vector3.Cross(Vector3.forward, shootDir);
        Vector3 forward = Vector3.forward;

        if(transform.rotation.y % 360 != 0)
        {
            forward = -forward;
            upwards = -upwards;
        }

        shootingPoint.rotation = Quaternion.LookRotation(forward, upwards);
    }

    void PickDirection()
    {
        int chance = Random.Range(1, 101);
        moveDir = chance <= 50 ? Vector2.left : Vector2.right;
    }

    public void Stop() => moveDir = Vector2.zero;

    public Bullet InstantiateBullet(Bullet bullet, Vector3 position, Quaternion rotation) => Instantiate(bullet, position, rotation);
    public Enemy InstantiateEnemy(Enemy enemy, Vector3 position, Quaternion rotation) => Instantiate(enemy, position, rotation);
    public void DestroyBullet(Bullet bullet, float t) => Destroy(bullet.gameObject, t);
    public void DestroyBullet(Bullet bullet) => Destroy(bullet.gameObject);
}
