using UnityEngine;

public class BulletRain : BossAttack
{
    public BulletRain(Boss boss, PlayerMovement player, Transform shootingPoint, BulletSO bullet) : base(boss, player, shootingPoint, bullet) {}

    const float CAMERA_MAX_HEIGHT = 17f / 2f;
//how long should the boss shoot straight up
    float shootingTime = 5;
    float shootingTimer;

    float fireRateTime = .1f;
    float fireRateTimer;

    //when should the bullet rain start
    float bulletRainStartTime = 3f;
    float bulletRainStartTimer;

    float rainFireRateTime = .1f;
    float rainFireRateTimer;

    //how long should the bullet rain last for
    float bulletRainTime = 5;
    float bulletRainTimer;

    public override void InitAttack()
    {
        base.InitAttack();
        fireRateTimer = fireRateTime;
        rainFireRateTimer = rainFireRateTime;
        shootingTimer = 0;
        bulletRainStartTimer = 0;
        bulletRainTimer = 0;
    }

    public override void Attack()
    {
        if(shootingTimer < shootingTime)
        {
            Quaternion cannonRotation = Quaternion.Euler(0, 0, 90);
            boss.RotateGun(cannonRotation);
            if (fireRateTimer >= fireRateTime)
            {
                fireRateTimer = 0;
                shootingTimer+= Time.deltaTime;
                Bullet bulletToShoot = boss.InstantiateBullet(bullet.bulletPrefab, shootingPoint.position, shootingPoint.rotation);
                InitBullet(bulletToShoot);
                boss.DestroyBullet(bulletToShoot, 2f);
            }
            else
            {
                shootingTimer+= Time.deltaTime;
                fireRateTimer += Time.deltaTime;
            }
        }
        else
            boss.TakeAim();

        bulletRainStartTimer+= Time.deltaTime;

        if(bulletRainStartTimer >= bulletRainStartTime)
        {
            if(bulletRainTimer >= bulletRainTime)
            {
                isAttackDone = true;
                return;
            }

            bulletRainTimer+= Time.deltaTime;

            if(rainFireRateTimer >= rainFireRateTime)
            {
                rainFireRateTimer = 0;
                Vector2 shootingPos = new(player.transform.position.x, CAMERA_MAX_HEIGHT + .2f);
                Quaternion bulletRotation = Quaternion.Euler(0, 0, -90);
                Bullet bulletToShoot = boss.InstantiateBullet(bullet.bulletPrefab, shootingPos, bulletRotation);
                InitBullet(bulletToShoot);
            }
            else
                rainFireRateTimer+= Time.deltaTime;
        }

    }
}
