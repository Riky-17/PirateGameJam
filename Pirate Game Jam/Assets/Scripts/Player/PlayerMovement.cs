using UnityEngine;

public class PlayerMovement : MonoBehaviour, IHealth, IItemPicker
{
    [HideInInspector] public Vector2 mousePos;
    Rigidbody2D rb;
    Vector2 moveInput;

    [SerializeField] float speed = 6;

    bool canMove = true;

    //weapon objects -> assigned in inspector
    [SerializeField] WeaponSystem[] weapons;
    WeaponSystem currentWeapon;

    //health system
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    [SerializeField] float maxHealth = 150;

    public float Health { get => health; set => health = value; }
    float health;

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
        health = maxHealth;
        currentWeapon = weapons[0];
        longColorFlash = new();
    }
    
    void Update()
    {
        ColorFlash();
        CheckStatBoost();
        GetMovementInput();
        MousePosition();
        SpriteRotation();
        GetWeaponInput();
    }

    void FixedUpdate() => Movement();

    void GetMovementInput()
    {
        moveInput = Vector2.zero;

        //getting the move input
        if (canMove)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                moveInput += Vector2.up;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                moveInput += Vector2.down;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                moveInput += Vector2.left;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                moveInput += Vector2.right;

            moveInput = moveInput.normalized;
        }
    }

    void ColorFlash()
    {
        if(shortColorFlash.duration > 0)
        {
            currentWeapon.ChangeSpriteColor(shortColorFlash.Color);
            shortColorFlash.duration-= Time.deltaTime;
            if(shortColorFlash.duration <= 0)
            {
                currentWeapon.ChangeSpriteColor(Color.white);
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

            currentWeapon.ChangeSpriteColor(longColorFlash.GetColor(intervalTimer));
            longColorFlash.ReduceDurations(Time.deltaTime);

            return;
        }

        currentWeapon.ChangeSpriteColor(Color.white);
    }

    void CheckStatBoost()
    {
        if(damageBoostTime != 0)
        {
            if (damageBoostTimer >= damageBoostTime)
            {
                WeaponSystem.DamageMultiplier = 1;
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
                WeaponSystem.FireRateMultiplier = 1;
                fireRateBoostTime = 0;
                fireRateBoostTimer = 0;
            }
            else
                fireRateBoostTimer+= Time.deltaTime;
        }
    }

    void GetWeaponInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchWeapon(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SwitchWeapon(3);
    }

    void SwitchWeapon(int index)
    {
        if (weapons[index] != currentWeapon)
        {
            WeaponSystem nextWeapon = weapons[index];
            nextWeapon.gameObject.SetActive(true);
            nextWeapon.ChangeSpriteColor(currentWeapon.WeaponSpriteColor());
            currentWeapon.ChangeSpriteColor(Color.white);
            currentWeapon.gameObject.SetActive(false);
            currentWeapon = nextWeapon;
        }
    }

    void MousePosition() => mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Gets Vector2 mouse position

    void Movement()
    {
        //physics calc
        Vector2 velocityInput = moveInput * speed;
        Vector3 velocityDiff = velocityInput - rb.linearVelocity;
        float accelRate = 7f;
        Vector3 force = velocityDiff * accelRate;
        rb.AddForce(force);
    }

    void SpriteRotation()
    {
        // Rotates sprite based on mouse position
        Vector2 mouseVector = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        float rotZ = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;

        if (rotZ >= -90 && rotZ <= 90)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            transform.localScale = new Vector2(1, -1);
        }
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        shortColorFlash = new(Color.green);
        if(health > maxHealth)
            health = maxHealth;
        Debug.Log(gameObject.name + " Health: " + health);
    }

    public void Damage(float damageAmount)
    {
        Debug.Log(damageAmount);
        health -= damageAmount;
        shortColorFlash = new(Color.red);
        Debug.Log(gameObject.name + " Health: " + health);
        if(health <= 0)
            Die();
    }

    //TODO
    public void Die()
    {
        
    }

    public void PickItem(PickableItem item) => item.Effect(this);

    public void DamageBoost(float DamageBoostAmount, float duration)
    {
        WeaponSystem.DamageMultiplier = DamageBoostAmount;
        damageBoostTime+= duration;
        longColorFlash.AddColor(new(0.2f, 0.6f, 1), duration);
    }

    public void FireRateBoost(float FireRateBoostAmount, float duration)
    {
        WeaponSystem.FireRateMultiplier = FireRateBoostAmount;
        fireRateBoostTime+= duration;
        longColorFlash.AddColor(Color.yellow, duration);
    }
}
