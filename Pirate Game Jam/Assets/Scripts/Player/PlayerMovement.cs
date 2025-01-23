using UnityEngine;

public class PlayerMovement : MonoBehaviour, IHealth, IItemPicker
{
    [HideInInspector] public Vector2 mousePos;
    Rigidbody2D rb;
    Vector2 moveInput;

    [SerializeField] float speed = 6;

    float speedBoost;
    float speedBoostDuration;
    float speedBoosTimer;

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
    float longColorFlashTimer = .2f;
    ShortColorFlash shortColorFlash;
    LongColorFlash longColorFlash;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        currentWeapon = weapons[0];
    }
    
    void Update()
    {
        ColorFlash();
        CheckSpeedBoost();
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
                if(longColorFlash.duration > 0)
                    longColorFlash.duration-= Time.deltaTime;
                return;
            }
        }

        if(longColorFlash.duration > 0)
        {
            longColorFlash.duration-= Time.deltaTime;
            longColorFlashTimer-= Time.deltaTime;

            if(longColorFlashTimer > .1f)
                currentWeapon.ChangeSpriteColor(longColorFlash.Color);
            else if(longColorFlashTimer <= 0)
                currentWeapon.ChangeSpriteColor(Color.white);
            
            if(longColorFlashTimer <= 0)
                longColorFlashTimer = .2f;

            if(longColorFlash.duration <= 0)
            {
                currentWeapon.ChangeSpriteColor(Color.white);
                longColorFlash = default;
            } 
            else
                return;
        }
    }

    void CheckSpeedBoost()
    {
        if(speedBoost != 0)
        {
            if(speedBoosTimer < speedBoostDuration)
                speedBoosTimer+= Time.deltaTime;
            else
            {
                speedBoosTimer = 0;
                speedBoostDuration = 0;
                speedBoost = 0;
            }
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
        Vector2 velocityInput = moveInput * (speed + speedBoost);
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

    //temporary for testing speed boost
    public void SpeedBoost(float speedBoostAmount, float duration)
    {
        speedBoost = speedBoostAmount;
        speedBoostDuration = duration;
        longColorFlash = new(Color.yellow, duration);
    }
}
