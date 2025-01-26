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

    public float DamageMultiplier => damageMultiplier;
    float damageMultiplier = 1;

    float fireRateMultiplier = 1;

    protected SpriteRenderer sr;

    public Vector2 CenterPoint => centerPoint;
    [SerializeField] Vector2 centerPoint;
    public Transform ShootingPoint => shootingPoint;
    [SerializeField] protected Transform shootingPoint;

    bool isDead = false;

    protected PlayerMovement player;

    protected List<BossAttack> attacks;
    protected BossAttack CurrentAttack => currentAttack;
    BossAttack currentAttack;

    bool isIdling = true;

    float idleTime = 2;
    float idleTimer;

    Vector2 moveDir;

    float walkTime = 3;
    float walkTimer;

    float waitTime = 1;
    float waitTimer;

    [Header("UI")]
    [SerializeField] public Slider bossHP;
    

    protected override void Awake()
    {
        base.Awake();
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();

        //doing it on Awake right now for testing
        Collider2D[] colliders = Physics2D.OverlapBoxAll(centerPoint, new(30, 17), 0);

        foreach (Collider2D collider in colliders)
        {
            if(collider.gameObject.TryGetComponent(out player))
                break;
        }

        if (bossHP != null)
        {
            bossHP.value = health;
            bossHP.maxValue = maxHealth;
            UpdatingHPSlider(health);
        }

        InitBoss();
    }

    void UpdatingHPSlider(float health)
    {
        bossHP.value = Mathf.Clamp(health, 0, bossHP.maxValue);
    }

    protected virtual void Update()
    {
        ColorFlash();
        if (isIdling)
        {
            TakeAim();
            if (idleTimer >= idleTime * fireRateMultiplier)
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
                currentAttack.Attack();
            else
            {
                if(moveDir == Vector2.zero)
                    PickDirection();
                currentAttack = null;
                isIdling = true;
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
        isIdling = false;
        Stop();
        OnDeath();
    }

    //this is called once when the boss is dead
    protected virtual void OnDeath() {}
    //this is called repeatedly after the boss is dead
    protected virtual void Dying() {}

    protected virtual void DeactivateSprite() => sr.enabled = false;

    public void PickItem(PickableItem item) => item.Effect(this);

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
    public void DestroyBullet(Bullet bullet, float t) => Destroy(bullet.gameObject, t);
    public void DestroyBullet(Bullet bullet) => Destroy(bullet.gameObject);
}
