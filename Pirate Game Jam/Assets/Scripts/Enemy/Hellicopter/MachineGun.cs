using UnityEngine;

public class MachineGun : BossAttack
{
    public MachineGun(Boss boss, PlayerMovement player, Transform shootingPoint, BulletSO bullet) : base(boss, player, shootingPoint, bullet) {}

    const float CAMERA_MAX_HEIGHT = 17f / 2f;

    //how long should the boss shoot straight up
    float shootingTime = 10;
    float shootingTimer;

    float fireRateTime = .05f;
    float fireRateTimer; 

    //innacuray from the machineGun 
    int sprayAngle = 20;
    public override void InitAttack()
    {
        base.InitAttack();
        fireRateTimer = fireRateTime;
        shootingTimer = 0;
    }
    
    public override void Attack()
    {
        shootingTimer += Time.deltaTime;
        if (shootingTimer < shootingTime)
        {
            if(fireRateTimer >= fireRateTime)
            {
                boss.TakeAim();
                //updating the Aim constantly so it fires the gun
                int inaccuracy = Random.Range(-sprayAngle, sprayAngle); ;
                Quaternion bulletSprite = Quaternion.Euler(0f, 0f, inaccuracy);
                Bullet bulletToShoot = boss.InstantiateBullet(bullet.bulletPrefab, shootingPoint.position, shootingPoint.rotation * bulletSprite);
                InitBullet(bulletToShoot);
                boss.DestroyBullet(bulletToShoot, 3f);

                fireRateTimer = 0;
            }
            fireRateTimer += Time.deltaTime;
                     
            
        }
        if (shootingTimer > shootingTime)
        {
            isAttackDone = true;
            shootingTimer = 0;
            return;
        }

    }

}
