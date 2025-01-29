using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HellicopBoss : Boss
{
    public static Action<Transform> onExplode;

    [Space]
    [SerializeField]SpriteRenderer srCannon;
    Transform trCannon;

    [Space]
    //weapons
    [SerializeField] BulletSO grenade;
    [SerializeField] BulletSO bullet;
    [SerializeField] BulletSO marksmanBullet;
    [SerializeField] BulletSO shotgunBullet;
    [SerializeField] BulletSO grenadeOverload;
    [SerializeField] Enemy enemyToSpawn;
    [SerializeField] Transform cannon;

    [Space]

    [SerializeField] PickUpItemSO[] Items;

    
    [Space]

    [SerializeField] GameObject deathExplosion;
    [SerializeField] float deathRadius;

    float deathTime = 3.5f;
    float deathTimer;

    float explosionTime = .3f;
    float explosionTimer;

    float afterDeathTime = 2.5f;
    float afterDeathTimer;

    bool canAttack = true;

    public List<Enemy> SpawnedEnemies { get; private set; }
    public List<PickableItem> itemsOnGame {  get; private set; }

    MachineGun machineGun;

    protected override void InitBoss()
    {
        SpawnedEnemies = new();
        itemsOnGame = new();
        sr = GetComponent<SpriteRenderer>();
        trCannon = GetComponentInChildren<Transform>();
     
        machineGun = new MachineGun(this, player, bullet);
        attacks = new()
        {
           //new Swoop(this, player, bullet, Items),
           new LandingCrash(this, player, bullet, enemyToSpawn, Items),
           //new CarpetBoom(this, player, grenade),
           //new OverloadedGun(this, player, marksmanBullet, grenadeOverload, shotgunBullet),
        };

    }
    protected override void Awake()
    {
        base.Awake();
       
    }
    protected override void LoadNextScene() => GameManager.Instance.LoadScene(3);
    protected override void UpdateColor(Color color)
    {
        sr.color = color;
        srCannon.color = color;
    }
    protected override void Update()
    {
        base.Update();
        
        if(canAttack)
            machineGun.Attack();
    }
    public override void TakeAim()
    {
        Vector2 shootDir = (player.transform.position - transform.position).normalized;
        Vector3 upwards = Vector3.Cross(Vector3.forward, shootDir);
        Vector3 forward = Vector3.forward;

        if(shootDir.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            upwards = -upwards;
            forward = - forward;
        }
        else
            transform.rotation = Quaternion.identity;

        cannon.rotation = Quaternion.LookRotation(forward, upwards);
    }

    protected override void DeactivateSprite()
    {
        sr.enabled = false;
        srCannon.enabled = false;
    }

    protected override void OnDeath()
    {
        
        canAttack = false;
        foreach (Enemy enemy in SpawnedEnemies)
        {
            if (enemy != null)
                enemy.Die();
        }
        foreach(PickableItem item in itemsOnGame)
        {
            if(item != null)
            {
                item.DisableItem();
            }
        }
    }
    protected override void Dying()
    {

        if (deathTimer < deathTime)
        {
            deathTimer += Time.deltaTime;
            if (explosionTimer >= explosionTime)
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
            if (afterDeathTimer < afterDeathTime)
            {
                afterDeathTimer += Time.deltaTime;
                return;
            }
            else
            {
                AudioManager.Instance.PlayMusic(3);
                LoadNextScene();
            }
        }
    }

    void DeathExplosion()
    {
        Vector2 pos = UnityEngine.Random.insideUnitCircle * deathRadius + (Vector2)transform.position;
        GameObject explosion = Instantiate(deathExplosion, pos, Quaternion.identity);
        onExplode?.Invoke(transform);
        Destroy(explosion, .3f);
    }
}
