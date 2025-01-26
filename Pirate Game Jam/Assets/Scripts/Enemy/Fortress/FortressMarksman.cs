using UnityEngine;

public class FortressMarksman : FortressWindow
{
    public FortressMarksman(Boss boss, Transform transform, BulletSO bulletSO) : base(boss, transform, bulletSO) { fireRateBaseTime = 1.5f; fireRateTimer = FireRateTime; }

    public override void Shoot()
    {
        if(fireRateTimer >= FireRateTime)
        {
            fireRateTimer = 0;
            Bullet bullet = boss.InstantiateBullet(bulletSO.bulletPrefab, Transform.position, Transform.rotation);
            InitBullet(bullet);
            boss.DestroyBullet(bullet, 2f);
        }
        else
            fireRateTimer+= Time.deltaTime;
    }
}
