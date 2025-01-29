using UnityEngine;

public class BossGrenadeLauncher : LivingGunBossGun
{
    public override void Shoot()
    {
        if(fireRateTimer >= FireRateTime)
        {
            fireRateTimer = 0;
            ShootBullet();
        }
        else
            fireRateTimer+= Time.deltaTime;
    }

    public override void ShootBullet(Vector2 pos, Quaternion rotation)
    {
        Bullet bullet = Instantiate(bulletSO.bulletPrefab, pos, rotation);
        InitBullet(bullet);
        Destroy(bullet.gameObject, 2f);
    }
}
