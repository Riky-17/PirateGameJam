using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Boss : ColorFlashObject, IHealth, IItemPicker
{
    const float MAX_CAMERA_WIDTH = 30 / 2;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    [SerializeField] float maxHealth = 500;
    public float Health { get => health; set => health = value; }
    float health;

    public float Speed => moveSpeed;

    public float DamageMultiplier => damageMultiplier;
    float damageMultiplier = 1;

    public float FireRateMultiplier => fireRateMultiplier;
    float fireRateMultiplier = 1;

    protected SpriteRenderer sr;

    public Vector2 CenterPoint => centerPoint;
    [SerializeField] protected Vector2 centerPoint;
    public Transform ShootingPoint => shootingPoint;
    [SerializeField] protected Transform shootingPoint;

    public static bool CanShoot;

    protected bool IsDead => isDead; 
    bool isDead = false;

    protected PlayerMovement player;

    protected List<BossAttack> attacks;

    protected BossAttack currentAttack;

    protected bool isIdle = true;

    float idleTime = 2;
    float idleTimer;

    protected Vector2 moveDir;

    float walkTime = 3;
    float walkTimer;

    float waitTime = 1;
    float waitTimer;

    [Header("UI")]
    [SerializeField] public Slider bossHP;
    
    //statBoostTimer
    float damageBoostTime;
    float damageBoostTimer;

    float fireRateBoostTime;
    float fireRateBoostTimer;
    

    protected override void Awake()
    {
        base.Awake();
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        this.centerPoint = new Vector2(this.transform.position.x - 11.5f, this.transform.position.y);
        //doing it on Awake right now for testing
        Collider2D[] colliders = Physics2D.OverlapBoxAll(centerPoint, new(60, 34), 0);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out player))
                break;
        }

        if(player == null)
            Debug.Log("Could not find player in Physics2D.OverlapBoxAll. Centerpoint: " + centerPoint);

        if (bossHP != null)
        {
            bossHP.value = health;
            bossHP.maxValue = maxHealth;
            UpdatingHPSlider(health);
        }

        InitBoss();
        PickDirection();
    }

    protected void UpdatingHPSlider(float health) => bossHP.value = Mathf.Clamp(health, 0, bossHP.maxValue);

    protected virtual void Update()
    {
        CheckStatBoost();
        ColorFlash();
        if (isIdle)
        {
            TakeAim();
            if (idleTimer >= idleTime * fireRateMultiplier)
            {
                isIdle = false;
                PickAttack();
            }
            else
                idleTimer += Time.deltaTime;
        }

        if(currentAttack != null && CanShoot)
        {
            if (!currentAttack.IsAttackDone)
                currentAttack.Attack();
            else
            {
                if(moveDir == Vector2.zero)
                    PickDirection();
                currentAttack = null;
                isIdle = true;
                idleTimer = 0;
            }
        }

        if(isDead)
            Dying();
    }

    void FixedUpdate()
    {
        if(moveDir == Vector2.zero)
        {
            AddForce(moveDir, moveSpeed, 3f);
            return;
        }

        if (walkTimer < walkTime)
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
        AddForce(moveDir, moveSpeed, 3);
    }

    protected void CheckStatBoost()
    {
        if(damageBoostTime != 0)
        {
            if (damageBoostTimer >= damageBoostTime)
            {
                damageMultiplier = 1;
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

    public void Heal(float healAmount)
    {
        if(health <= 0)
            return;
        
        health+= healAmount;
        shortColorFlash = new(Color.green);
        if (health > maxHealth)
            health = maxHealth;
        Debug.Log(gameObject.name + "Health: " + health);

        UpdatingHPSlider(health);

    }

    public void Damage(float damageAmount)
    {
        if(health <= 0)
            return;

        health-= damageAmount;
        shortColorFlash = new(Color.red);
        Debug.Log(gameObject.name + "Health: " + health);
        UpdatingHPSlider(health);

        if (health <= 0)   
        Die();
    }

    public void Die()
    {
        isDead = true;
        currentAttack = null;
        isIdle = false;
        Stop();
        OnDeath();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IHealth damageable = collision.gameObject.GetComponent<IHealth>();
        Rigidbody2D collisionRB = collision.gameObject.GetComponent<Rigidbody2D>();
        Enemy collisionEnemy = collision.gameObject.GetComponent<Enemy>();
        if (damageable != null && collisionRB != null)
        {
            Vector2 forceVector = (collision.gameObject.transform.position - transform.position).normalized * 5f; // Direction and force to be knockbacked for collision object
            collisionRB.AddForce(forceVector, ForceMode2D.Impulse);
        }
        if (collisionEnemy != null)
        {
            collisionEnemy.Damage(100f);
        }
    }

    //this is called once when the boss is dead
    protected virtual void OnDeath() {}
    //this is called repeatedly after the boss is dead
    protected virtual void Dying() {}

    protected virtual void DeactivateSprite() => sr.enabled = false;

    public void PickItem(PickableItem item) => item.Effect(this);

    public void DamageBoost(float damageBoostAmount, float duration)
    {
        damageMultiplier = damageBoostAmount;
        damageBoostTime+= duration;
        LongColorFlash.AddColor(new(0.2f, 0.6f, 1), duration);
    }

    public void FireRateBoost(float fireRateBoostAmount, float duration)
    {
        fireRateMultiplier = fireRateBoostAmount;
        fireRateBoostTime+= duration;
        LongColorFlash.AddColor(Color.yellow, duration);
    }

    protected abstract void InitBoss();
    protected abstract void LoadNextScene();

    protected virtual void PickAttack()
    {
        currentAttack = attacks[Random.Range(0, attacks.Count)];
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

    public void AddForceBoss(Vector2 dir, float speed, float accelRate) => AddForce(dir, speed, accelRate);

    public void Stop() => moveDir = Vector2.zero;

    public Bullet InstantiateBullet(Bullet bullet, Vector3 position, Quaternion rotation) => Instantiate(bullet, position, rotation);
    public Enemy InstantiateEnemy(Enemy enemy, Vector3 position, Quaternion rotation) => Instantiate(enemy, position, rotation);
    public PickableItem InstantiateItem(PickableItem item, Vector3 position, Quaternion rotation) => Instantiate(item, position, rotation);
    public GameObject InstantiateObject(GameObject gameObject, Vector3 position, Quaternion rotation) => Instantiate(gameObject, position, rotation);
    public void DestroyBullet(Bullet bullet, float t) => Destroy(bullet.gameObject, t);
    public void DestroyBullet(Bullet bullet) => Destroy(bullet.gameObject);
    public void DestroyObject(GameObject gameObject, float t) => Destroy(gameObject, t);
    public void DestroyObject(GameObject gameObject) => Destroy(gameObject);
}
