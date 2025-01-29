using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingGunBoss : Boss
{
    public static Action<Transform> onExplode;

    List<LivingGunBossGun> weapons;
    [SerializeField] BossMarksman marksmanGun;
    [SerializeField] BossAssaultRifle assaultRifleGun;
    [SerializeField] BossGrenadeLauncher grenadeLauncherGun;
    [SerializeField] BossShotgun shotgunGun;
    LivingGunBossGun currentWeapon;

    Dictionary<LivingGunBossGun, BossAttack> gunAttacks;

    float weaponTime = 5f;
    float weaponTimer;

    public bool canMove = true;
    public bool canAutoAttack = true;

    float idlingTime = 5f;
    float idlingTimer;

    Vector2 bossToPlayer;

    float deathTime = 3;
    float deathTimer;

    float explosionTime = .3f;
    float explosionTimer;

    [SerializeField] GameObject deathExplosion;

    float afterDeathTime = 2.5f;
    float afterDeathTimer;

    protected override void InitBoss()
    {
        weapons = new()
        {
            marksmanGun,
            assaultRifleGun,
            grenadeLauncherGun,
            shotgunGun,
        };

        foreach (LivingGunBossGun weapon in weapons)
            weapon.InitGun(this);

        gunAttacks = new()
        {
            { marksmanGun, new MarksmanAttack(this, player, marksmanGun) },
            { assaultRifleGun, new AssaultRifleAttack(this, player, assaultRifleGun) },
            { grenadeLauncherGun, new GrenadeLauncherAttack(this, player, grenadeLauncherGun) },
            { shotgunGun, new ShotgunAttack(this, player, shotgunGun) }
        };

        currentWeapon = weapons[0];
        GetDir();
    }

    protected override void Update()
    {
        ColorFlash();
        GetDir();

        if(canAutoAttack)
        {
            TakeAim();   
            currentWeapon.Shoot();
        }

        if(isIdle)
        {
            if(idlingTimer >= idlingTime)
            {
                isIdle = false;
                idlingTimer = 0;
                PickAttack();
            }
            else
                idlingTimer+= Time.deltaTime;
        }

        if (currentAttack == null)
        {
            if(weaponTimer < weaponTime)
                weaponTimer+= Time.deltaTime;
            else
            {
                weaponTimer = 0;
                SwitchWeapon();
            }
        }
        else
        {
            if(currentAttack.IsAttackDone)
            {
                currentAttack = null;
                isIdle = true;
            }
            else
                currentAttack.Attack();
        }

        if(IsDead)
            Dying();
    }

    void FixedUpdate()
    {
        if(!canMove)
        {
            AddForce(moveDir, 0, 10);
            return;
        }

        Vector2 bossToPlayerFlat = new(0, player.transform.position.y - transform.position.y);

        if(bossToPlayerFlat.magnitude < .1f)
            moveDir = Vector2.zero;
        
        AddForce(moveDir, moveSpeed, 5);
    }

    public override void TakeAim()
    {
        if(bossToPlayer.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.identity;

        Vector3 upwards = Vector3.Cross(Vector3.forward, bossToPlayer.normalized);
        Vector3 forward = Vector3.forward;

        if(transform.rotation.y % 360 != 0)
        {
            forward = -forward;
            upwards = -upwards;
        }

        transform.rotation = Quaternion.LookRotation(forward, upwards);
    }

    void SwitchWeapon()
    {
        int randomWeaponIndex = UnityEngine.Random.Range(0, weapons.Count);
        if(weapons[randomWeaponIndex] == currentWeapon)
            return;

        LivingGunBossGun nextWeapon = weapons[randomWeaponIndex];
        nextWeapon.gameObject.SetActive(true);
        nextWeapon.ChangeWeaponColor(currentWeapon.WeaponColorSprite());
        nextWeapon.ResetFireRate();
        currentWeapon.ChangeWeaponColor(Color.white);
        currentWeapon.gameObject.SetActive(false);
        currentWeapon = nextWeapon;
    }

    void GetDir()
    {
        bossToPlayer = player.transform.position - transform.position;
        moveDir = new(0, player.transform.position.y - transform.position.y);
        moveDir = moveDir.normalized;
    }

    protected override void PickAttack()
    {
        currentAttack = gunAttacks[currentWeapon]; 
        currentAttack.InitAttack();
    }

    public void StopAngularVelocity() => rb.angularVelocity = 0;

    public void SetHPSlider(Slider HPSlider)
    {
        bossHP = HPSlider;
        bossHP.value = Health;
        bossHP.maxValue = MaxHealth;
        UpdatingHPSlider(Health);
    }

    protected override void OnDeath()
    {
        canMove = false;
        canAutoAttack = false;
    }

    protected override void Dying()
    {if (deathTimer < deathTime)
        {
            deathTimer+= Time.deltaTime;
            if(explosionTimer >= explosionTime)
            {
                explosionTimer = 0;
                DeathExplosion();
                return;
            }
            else
            {
                explosionTimer += Time.deltaTime;
                return;
            }
        }
        else
        {
            DeactivateSprite();
            if(afterDeathTimer < afterDeathTime)
            {
                afterDeathTimer+= Time.deltaTime;
                return;
            }
            else
                LoadNextScene();
        }
    }

    protected override void DeactivateSprite() => currentWeapon.gameObject.SetActive(false);

    void DeathExplosion()
    {
        Vector2 pos = UnityEngine.Random.insideUnitCircle + (Vector2)transform.position;
        GameObject explosion = Instantiate(deathExplosion, pos, Quaternion.identity);
        onExplode?.Invoke(transform);
        Destroy(explosion, .3f);
    }

    public override void RotateGun(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    protected override void LoadNextScene()
    {
        GameManager.Instance.LoadScene(4);
    }

    protected override void UpdateColor(Color color)
    {
        currentWeapon.ChangeWeaponColor(color);
    }
}
