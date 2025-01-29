using System.Collections.Generic;
using UnityEngine;

public class FortressBoss : Boss
{
    [SerializeField] Sprite attackSprite;
    [SerializeField] Sprite deathSprite;

    [SerializeField] List<Transform> windowsTrfs;
    [SerializeField] List<Transform> weapons;
    [SerializeField] Transform livingGunSpawnPos;
    List<FortressWindow> windows;
    [SerializeField] BulletSO grenade;
    [SerializeField] BulletSO shotgunBullet;
    [SerializeField] BulletSO assaultRifleBullet;
    [SerializeField] BulletSO marksmanBullet;

    [SerializeField] BossParachuteEnemy enemy;

    [SerializeField] GameObject deathExplosion;

    [SerializeField] LivingGunBoss livingGunBoss;

    public List<Enemy> SpawnedEnemies { get; private set;}

    bool canAttack = true;

    float deathTime = 3.5f;
    float deathTimer;

    float explosionTime = .3f;
    float explosionTimer;

    float afterDeathTime = 2.5f;
    float afterDeathTimer;

    bool weaponSpritesActive = false;

    float weaponsShowTime = 1f;
    float weaponsShowTimer;

    bool areWeaponsInPos = false;
    bool livingWeaponExplosion = false;
    float bossGunExplosionTime = .3f;
    float bossGunExplosionTimer;

    protected override void InitBoss()
    {
        sr.GetComponent<SpriteRenderer>();
        sr.sprite = attackSprite;
        SpawnedEnemies = new();

        windows = new()
        {
            new FortressAssaultRifle(this, windowsTrfs[0], assaultRifleBullet),
            new FortressMarksman(this, windowsTrfs[1], marksmanBullet),
            new FortressShotgun(this, windowsTrfs[2], shotgunBullet),
            new FortressGrenadeLauncher(this, windowsTrfs[3], grenade),
        };

        attacks = new()  
        {
            new FortressOverclock(this, player, null, enemy),
        };
    }

    protected override void PickAttack()
    {
        base.PickAttack();
        FortressWindow randomWindow = windows[Random.Range(0, windows.Count)];
        if(currentAttack is FortressOverclock overclock)
            overclock.SetWindow(randomWindow);
        shootingPoint = randomWindow.Transform;
    }

    protected override void Update()
    {
        base.Update();
        if(!canAttack)
            return;
        TakeAim();

        foreach (FortressWindow weapon in windows)
        {
            if(!weapon.isSpecialAttacking)
                weapon.Shoot();
        }
    }

    //removing the base fixedUpdate
    void FixedUpdate() {}

    public override void TakeAim()
    {
        foreach (FortressWindow weapon in windows)
        {
            if(!weapon.isSpecialAttacking)
                weapon.TakeAim(player);
        }
    }

    protected override void OnDeath()
    {
        GameManager.Instance.ClearBossEnemies();
        canAttack = false;
    }

    protected override void Dying()
    {
        if (deathTimer < deathTime)
        {
            deathTimer+= Time.deltaTime;
            if(explosionTimer >= explosionTime)
            {
                explosionTimer = 0;
                foreach (FortressWindow window in windows)
                    window.Explode(deathExplosion);
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
            explosionTimer = 0;
            sr.sprite = deathSprite;
            if(afterDeathTimer < afterDeathTime)
            {
                afterDeathTimer+= Time.deltaTime;
                return;
            }
            else
            {
                if(!weaponSpritesActive)
                {
                    for (int i = 0; i < weapons.Count; i++)
                    {
                        Transform window = windowsTrfs[i];
                        window.rotation = Quaternion.identity;
                        Transform weapon = weapons[i];
                        weapon.gameObject.SetActive(true);
                    }
                    weaponSpritesActive = true;
                    return;
                }
                
                if(weaponsShowTimer < weaponsShowTime)
                {
                    weaponsShowTimer+= Time.deltaTime;
                    return;
                }
                else if(!areWeaponsInPos)
                {
                    MoveWeapons();
                    return;
                }
                else
                {
                    if (!livingWeaponExplosion)
                    {
                        livingWeaponExplosion = true;
                        GameObject explosion = Instantiate(deathExplosion, livingGunSpawnPos.position, Quaternion.identity);
                        Destroy(explosion, 0.3f);
                        foreach (Transform weapon in weapons)
                            weapon.gameObject.SetActive(false);
                    }

                    if(bossGunExplosionTimer < bossGunExplosionTime)
                    {
                        bossGunExplosionTimer+= Time.deltaTime;
                        return;
                    }
                    else
                    {
                        LivingGunBoss bossGun = Instantiate(livingGunBoss, livingGunSpawnPos.position, Quaternion.identity);
                        bossGun.SetHPSlider(bossHP);
                        Destroy(this);
                    }
                }
            }
        }
    }

    void MoveWeapons()
    {
        foreach (Transform weapon in weapons)
        {
            Vector2 dir = livingGunSpawnPos.position - weapon.position;

            if(dir.magnitude < .1f)
            {
                areWeaponsInPos = true;
                continue;
            }

            areWeaponsInPos = false;
            dir = dir.normalized;
            weapon.transform.position += 3 * Time.deltaTime * (Vector3)dir;
        }
    }

    protected override void LoadNextScene()
    {
        
    }

    protected override void UpdateColor(Color color) => sr.color = color;
}
