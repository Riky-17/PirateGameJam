using System;
using UnityEngine;

public class TankBoss : Boss
{
    public static Action<Transform> onExplode;

    const float CAMERA_MAX_WIDTH = 30 / 2f;

    SpriteRenderer srCannon;
    [Space]
    [SerializeField] BulletSO grenade;
    [SerializeField] BulletSO bullet;
    [SerializeField] Enemy enemyToSpawn;
    [SerializeField] Transform cannon;

    float leftDirChances;

    [SerializeField] GameObject deathExplosion;
    [SerializeField] float deathRadius;

    float deathTime = 3.5f;
    float deathTimer;

    float explosionTime = .3f;
    float explosionTimer;

    float afterDeathTime = 2.5f;
    float afterDeathTimer;

    protected override void InitBoss()
    {
        sr = GetComponent<SpriteRenderer>();
        srCannon = cannon.GetComponent<SpriteRenderer>();
        explosionTimer = explosionTime;

        attacks = new() 
        { 
            new GrenadeBarrage(this, player, shootingPoint, grenade), 
            new BulletRain(this, player, shootingPoint, bullet), 
            new SignalFlare(this, player, grenade, enemyToSpawn),
            new SprayAndPray(this, player, shootingPoint, bullet),
        };
    }

    public override void RotateGun(Quaternion rotation) => cannon.rotation = rotation;

    protected override void DeactivateSprite()
    {
        sr.enabled = false;
        srCannon.enabled = false;
    }

    protected override void LoadNextScene() => GameManager.Instance.LoadScene(2);

    public override void TakeAim()
    {
        Vector2 shootDir = (player.transform.position - transform.position).normalized;
        Vector3 upwards = Vector3.Cross(Vector3.forward, shootDir);
        Vector3 forward = Vector3.forward;

        cannon.rotation = Quaternion.LookRotation(forward, upwards);
    }

    protected override void PickDirection()
    {
        CalculateDirectionChances();
        int chance = UnityEngine.Random.Range(1, 101);

        moveDir = chance <= leftDirChances ? Vector2.left : Vector2.right;
    }

    void CalculateDirectionChances()
    {
        float currentX = transform.position.x;
        float originX = centerPoint.x;
        float leftMostX = originX - (CAMERA_MAX_WIDTH + 1.5f);
        float rightMostX = originX + (CAMERA_MAX_WIDTH - 1.5f);

        leftDirChances =  Mathf.RoundToInt(100 * Mathf.InverseLerp(leftMostX, rightMostX, currentX));
    }

    protected override void UpdateColor(Color color)
    {
        sr.color = color;
        srCannon.color = color;
    }

    protected override void OnDeath() => GameManager.Instance.ClearBossEnemies();

    protected override void Dying()
    {
        if (deathTimer < deathTime)
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
            {
                AudioManager.Instance.PlayMusic(2);
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
