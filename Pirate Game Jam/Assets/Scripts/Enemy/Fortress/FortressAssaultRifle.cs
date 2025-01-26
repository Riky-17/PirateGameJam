using UnityEngine;

public class FortressAssaultRifle : FortressWindow
{
    public FortressAssaultRifle(Boss boss, Transform transform, BulletSO bulletSO) : base(boss, transform, bulletSO) { fireRateBaseTime = .1f; fireRateTimer = FireRateTime; }

    float shootTime = 3f;
    float shootTimer;

    float restTime = 2f;
    float restTimer;

    float sprayAngle = 20;

    public override void Shoot()
    {
        if(shootTimer < shootTime)
        {
            if(fireRateTimer >= FireRateTime)
            {
                fireRateTimer = 0;
                shootTimer+= Time.deltaTime;
                float inaccuracy = Random.Range(-sprayAngle, sprayAngle);
                Quaternion bulletRot = Transform.rotation * Quaternion.Euler(0, 0, inaccuracy);
                Bullet bullet = boss.InstantiateBullet(bulletSO.bulletPrefab, Transform.position, bulletRot);
                InitBullet(bullet);
                boss.DestroyBullet(bullet, 3f);
            }
            else
            {
                fireRateTimer+= Time.deltaTime;
                shootTimer+= Time.deltaTime;
            }
        }
        else
        {
            if(restTimer < restTime)
                restTimer+= Time.deltaTime;
            else
            {
                shootTimer = 0;
                restTimer = 0;
            }
        }
    }
}
