using UnityEngine;

public class MachineGun : BossAttack
{
    public MachineGun(Boss boss, PlayerMovement player, BulletSO bullet) : base(boss, player, bullet) {}

    const float CAMERA_MAX_HEIGHT = 17f / 2f;

    //how long should the boss shoot straight up
    float shootingTime = 10;
    float shootingTimer;

    float fireRateTime = .1f;
    float fireRateTimer; 

    //inaccuracy from the machineGun 
    int sprayAngle = 20;

    public override void InitAttack()
    {
        base.InitAttack();
        fireRateTimer = fireRateTime;
        shootingTimer = 0;
    }
    
    public override void Attack()
    {
        boss.TakeAim();
        if (shootingTimer < shootingTime)
        {
            shootingTimer += Time.deltaTime;
            if (fireRateTimer >= fireRateTime)
            {
                //updating the Aim constantly so it fires the gun
                int inaccuracy = Random.Range(-sprayAngle, sprayAngle); ;
                Quaternion bulletSprite = Quaternion.Euler(0f, 0f, inaccuracy);
                Bullet bulletToShoot = boss.InstantiateBullet(bullet.bulletPrefab, boss.ShootingPoint.position, boss.ShootingPoint.rotation * bulletSprite);
                InitBullet(bulletToShoot);
                boss.DestroyBullet(bulletToShoot, 3f);

                fireRateTimer = 0;
            }
            else
                fireRateTimer += Time.deltaTime;
        }
        else
            isAttackDone = true;
    }

}
