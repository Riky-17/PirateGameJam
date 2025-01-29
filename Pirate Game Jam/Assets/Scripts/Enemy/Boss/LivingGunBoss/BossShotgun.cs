using UnityEngine;

public class BossShotgun : LivingGunBossGun
{
    int DegShotgunAngle = 60;
    float DegShotgunHalfAngle => DegShotgunAngle / 2;
    int bulletsToShoot = 8;

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
        int tempBulletsAmount = bulletsToShoot;
    
        float degBulletsAngle= DegShotgunAngle / (tempBulletsAmount - 1);

        //getting half of the bullets
        tempBulletsAmount/= 2;

        for (int i = 0; i < tempBulletsAmount; i++)
        {
            float bulletDegAngle = DegShotgunHalfAngle - (degBulletsAngle * i);
            float bulletRadAngle = bulletDegAngle * Mathf.Deg2Rad;
            float x = Mathf.Cos(bulletRadAngle);
            float y = Mathf.Sin(bulletRadAngle);
            Vector2 bulletDir = new(x, y);
            Vector2 mirroredBulletDir = new(x, -y);

            Vector2 bulletUp = new(-bulletDir.y, bulletDir.x);
            Vector2 mirroredBulletUp = new(-mirroredBulletDir.y, mirroredBulletDir.x);

            Quaternion bulletRot = rotation * Quaternion.LookRotation(Vector3.forward, bulletUp);
            Quaternion mirroredBulletRot = rotation * Quaternion.LookRotation(Vector3.forward, mirroredBulletUp);

            Bullet tempBullet = Instantiate(bulletSO.bulletPrefab, pos, bulletRot);
            InitBullet(tempBullet);
            Destroy(tempBullet.gameObject, 5f);

            Bullet mirroredTempBullet = Instantiate(bulletSO.bulletPrefab, pos, mirroredBulletRot);
            InitBullet(mirroredTempBullet);
            Destroy(mirroredTempBullet.gameObject, 5f);
        }
    }
}
