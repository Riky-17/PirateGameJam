using UnityEngine;

public class OverloadedGun : BossAttack
{
    public OverloadedGun(Boss boss, PlayerMovement player, BulletSO bullet, BulletSO grenade, BulletSO shotgun) : base(boss, player, bullet)
    {
        GrenadeBullet = grenade;
        shotgunBullet = shotgun;
    }

    const float CAMERA_MAX_HEIGHT = 17 / 2f;
    const float CAMERA_MAX_WIDTH = 30 / 2f;

    BulletSO GrenadeBullet;
    BulletSO shotgunBullet;

    float fireRateTimerMarksman;
    float fireRateTimeMarksman = 1f;

    float fireRateTimerGrenade;
    float fireRateTimeGrenade = 3f;

    float fireRateTimerShotgun;
    float fireRateTimeShotgun = 2f;

    float shootingTimer;
    float shootingTime = 7f;


    public override void InitAttack()
    {
        base.InitAttack();
        fireRateTimerMarksman = fireRateTimeMarksman;
        fireRateTimerGrenade = fireRateTimeGrenade;
        fireRateTimerShotgun = fireRateTimeShotgun;
        shootingTimer = 0;
    }
    public override void Attack()
    {
        boss.TakeAim();

        if (shootingTimer < shootingTime)
        {
            shootingTimer += Time.deltaTime;
            //make him shoot ha ha ha ha 
            if (fireRateTimerMarksman >= fireRateTimeMarksman)
            {
                Bullet marksman = boss.InstantiateBullet(bullet.bulletPrefab, shootingPoint.position, shootingPoint.rotation);
                InitBullet(marksman);
                boss.DestroyBullet(marksman, 5f);
                fireRateTimerMarksman = 0;
            }
            else
                fireRateTimerMarksman += Time.deltaTime;

            if (fireRateTimerGrenade >= fireRateTimeGrenade)
            {
                Bullet grenade = boss.InstantiateBullet(GrenadeBullet.bulletPrefab, shootingPoint.position, shootingPoint.rotation);
                InitBullet(grenade);
                boss.DestroyBullet(grenade, 5f);
                fireRateTimerGrenade = 0;
            }
            else
                fireRateTimerGrenade += Time.deltaTime;

            if (fireRateTimerShotgun >= fireRateTimerGrenade)
            {
                Bullet shotgunBull = boss.InstantiateBullet(shotgunBullet.bulletPrefab, shootingPoint.position, shootingPoint.rotation);
                InitBullet(shotgunBull);
                boss.DestroyBullet(shotgunBull, 3f);
                fireRateTimerShotgun = 0;
            }
            else
                fireRateTimerGrenade += Time.deltaTime;
        }
        else
            isAttackDone = true;

    }
}
