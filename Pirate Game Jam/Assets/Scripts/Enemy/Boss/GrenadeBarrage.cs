using UnityEngine;

public class GrenadeBarrage : BossAttack
{
    public GrenadeBarrage(Boss boss, PlayerMovement player, Transform shootingPoint, BulletSO bullet) : base(boss, player, shootingPoint, bullet) { }

    int grenadeAmount = 7;
    int grenadeCount;
    float sprayAngle = 20;

    float fireRateTime = .3f;
    float fireRateTimer;

    public override void InitAttack()
    {
        base.InitAttack();
        boss.Stop();
        fireRateTimer = fireRateTime;
        grenadeCount = 0;
        fireRateTimer = 0;
    }

    public override void Attack()
    {
        boss.TakeAim();
        if(fireRateTimer >= fireRateTime)
        {
            fireRateTimer = 0;
            grenadeCount++;
        }
        else
        {
            fireRateTimer+= Time.deltaTime;
            return;
        }

        float inaccuracy = Random.Range(-sprayAngle, sprayAngle);
        Quaternion bulletRotation = shootingPoint.rotation * Quaternion.Euler(0, 0, inaccuracy);
        Bullet bulletToShoot = boss.InstantiateBullet(bullet.bulletPrefab, shootingPoint.position, bulletRotation);
        InitBullet(bulletToShoot);
        boss.DestroyBullet(bulletToShoot, .75f);

        if(grenadeCount >= grenadeAmount)
            isAttackDone = true;
    }

    
}
