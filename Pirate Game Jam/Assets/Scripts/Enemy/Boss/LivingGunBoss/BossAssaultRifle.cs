using UnityEngine;

public class BossAssaultRifle : LivingGunBossGun
{
    float bulletTime = .1f;
    float bulletTimer;

    float waitTime = 2;
    float waitTimer;

    float sprayAngle = 20;

    void Awake() => bulletTimer = bulletTime;

    public override void Shoot()
    {
        if(fireRateTimer < FireRateTime)
        {
            fireRateTimer+= Time.deltaTime;

            if(bulletTimer >= bulletTime)
            {
                bulletTimer = 0;
                ShootBullet();
            }
            else
                bulletTimer+= Time.deltaTime;
        }
        else
        {
            if(waitTimer >= waitTime)
            {
                waitTimer = 0;
                fireRateTimer = 0;
            }
            else
                waitTimer+= Time.deltaTime;
        }
    }

    public override void ShootBullet(Vector2 pos, Quaternion rotation)
    {
        float inaccuracy = Random.Range(-sprayAngle, sprayAngle);
        Quaternion bulletRot = rotation * Quaternion.Euler(0, 0, inaccuracy);
        Bullet bullet = Instantiate(bulletSO.bulletPrefab, pos, bulletRot);
        InitBullet(bullet);
        Destroy(bullet.gameObject, 5f);
    }
}
