using UnityEngine;

public class FortressShotgun : FortressWindow
{
    public FortressShotgun(Boss boss, Transform transform, BulletSO bulletSO) : base(boss, transform, bulletSO) { fireRateBaseTime = 1.5f; fireRateTimer = FireRateTime; }

    int DegShotgunAngle = 60;
    float DegShotgunHalfAngle => DegShotgunAngle / 2;
    int bulletsToShoot = 8;

    public override void Shoot()
    {
        if (fireRateTimer >= FireRateTime)
        {
            fireRateTimer = 0;
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
    
                Quaternion bulletRot = Transform.rotation * Quaternion.LookRotation(Vector3.forward, bulletUp);
                Quaternion mirroredBulletRot = Transform.rotation * Quaternion.LookRotation(Vector3.forward, mirroredBulletUp);
    
                Bullet tempBullet = boss.InstantiateBullet(bulletSO.bulletPrefab, Transform.position, bulletRot);
                InitBullet(tempBullet);
                boss.DestroyBullet(tempBullet, 3f);
    
                Bullet mirroredTempBullet = boss.InstantiateBullet(bulletSO.bulletPrefab, Transform.position, mirroredBulletRot);
                InitBullet(mirroredTempBullet);
                boss.DestroyBullet(mirroredTempBullet, 3f);
            }
        }
        else
            fireRateTimer+= Time.deltaTime;
    }
}
