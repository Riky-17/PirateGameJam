using UnityEngine;

public class FortressGrenadeLauncher : FortressWindow
{
    public FortressGrenadeLauncher(Boss boss, Transform transform, BulletSO bulletSO) : base(boss, transform, bulletSO) { fireRateBaseTime = 2.5f; fireRateTimer = FireRateTime;  }

    public override void Shoot()
    {
        if(fireRateTimer >= FireRateTime)
        {
            fireRateTimer = 0;
            Bullet bullet = boss.InstantiateBullet(bulletSO.bulletPrefab, Transform.position, Transform.rotation);
            InitBullet(bullet);
            boss.DestroyBullet(bullet, 1.5f);
        }
        else
            fireRateTimer+= Time.deltaTime;
    }
}
