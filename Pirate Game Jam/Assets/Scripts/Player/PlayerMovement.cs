using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : ColorFlashObject, IHealth, IItemPicker
{
    [HideInInspector] public Vector2 mousePos;

    Vector2 moveInput;

    [SerializeField] float speed = 6;

    //weapon objects -> assigned in inspector
    [SerializeField] WeaponSystem[] weapons;
    WeaponSystem currentWeapon;

    //health system
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    [SerializeField] float maxHealth = 150;

    public float Health { get => health; set => health = value; }
    float health;

    public static Action playerDies;

    //statBoostTimer
    float damageBoostTime;
    float damageBoostTimer;

    float fireRateBoostTime;
    float fireRateBoostTimer;

    float reloadBoostTime;
    float reloadBoostTimer;

    float bulletAmountMultiplier = 1;
    float bulletAmountBoostTime;
    float bulletAmountBoostTimer;

    //UI
    [Header("UI")]
    [SerializeField] private Slider HPSlider;
    [SerializeField] private TMP_Text HPText;
    protected override void Awake()
    {
        base.Awake();
        health = maxHealth;
        currentWeapon = weapons[0];
        HPSlider.maxValue = maxHealth;


        HPSlider.value = health;
        updatingHPSlider(health);

    }

    void OnEnable() => BalancedSliderController.onLevelUp += LevelUp;

    void OnDisable() => BalancedSliderController.onLevelUp -= LevelUp;

    void Update()
    {
        ColorFlash();
        CheckStatBoost();
        GetMovementInput();
        MousePosition();
        SpriteRotation();
        GetWeaponInput();
    }

    void FixedUpdate() => AddForce(moveInput, speed, 7f);

    void GetMovementInput()
    {
        moveInput = Vector2.zero;

        //getting the move input
        if (PanelsManager.canReadInput)
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

    void CheckStatBoost()
    {
        if(damageBoostTime != 0)
        {
            if (damageBoostTimer >= damageBoostTime)
            {
                WeaponSystem.DamageBoostMultiplier = 1;
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

        if(reloadBoostTime != 0)
        {
            if (reloadBoostTimer >= reloadBoostTime)
            {
                WeaponSystem.ReloadMultiplier = 1;
                reloadBoostTime = 0;
                reloadBoostTimer = 0;
            }
            else
                reloadBoostTimer+= Time.deltaTime;
        }

        if(bulletAmountBoostTime != 0)
        {
            if (bulletAmountBoostTimer >= bulletAmountBoostTime)
            {
                foreach (WeaponSystem weapon in weapons)
                    weapon.MultiplyBullet(1 / bulletAmountMultiplier);

                bulletAmountMultiplier = 1;
                bulletAmountBoostTime = 0;
                bulletAmountBoostTimer = 0;
            }
            else
                bulletAmountBoostTimer+= Time.deltaTime;
        }
    }

    void LevelUp()
    {
        foreach (WeaponSystem weapon in weapons)
            weapon.UpgradeStats();
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
        updatingHPSlider(health);
    }
    
    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        shortColorFlash = new(Color.red);
        updatingHPSlider(health);
        if(health <= 0)
            Die();
    }

    //TODO
    public void Die()
    {
        playerDies?.Invoke();
        updatingHPSlider(health);
    }

    protected override void UpdateColor(Color color) => currentWeapon.ChangeSpriteColor(color);

    public void PickItem(PickableItem item) => item.Effect(this);

    public void DamageBoost(float damageBoostAmount, float duration)
    {
        WeaponSystem.DamageBoostMultiplier = damageBoostAmount;
        damageBoostTime+= duration;
        LongColorFlash.AddColor(new(0.2f, 0.6f, 1), duration);
    }

    public void FireRateBoost(float fireRateBoostAmount, float duration)
    {
        WeaponSystem.FireRateMultiplier = fireRateBoostAmount;
        fireRateBoostTime+= duration;
        LongColorFlash.AddColor(Color.yellow, duration);
    }

    public void ReloadBoost(float reloadBoostAmount, float duration)
    {
        WeaponSystem.ReloadMultiplier = reloadBoostAmount;
        reloadBoostTime+= duration;
        LongColorFlash.AddColor(Color.magenta, duration);
    }

    public void BulletAmountBoost(float bulletAmountBoost, float duration)
    {
        bulletAmountMultiplier = bulletAmountBoost;
        foreach (WeaponSystem weapon in weapons)
            weapon.MultiplyBullet(bulletAmountMultiplier);

        bulletAmountBoostTime+= duration;
        LongColorFlash.AddColor(Color.grey, duration);
    }

    void updatingHPSlider(float health)
    {
        HPSlider.value = Mathf.Clamp(health, 0, HPSlider.maxValue);
        HPText.text = "HP: " + HPSlider.value.ToString();
    }
    
}
