using System;
using UnityEngine;

public class ShotgunScript : WeaponSystem
{
    public static Action<Transform> onShotgunPump;

    int DegShotgunAngle = 60;
    float DegShotgunHalfAngle => DegShotgunAngle / 2;
    int bulletsToShoot = 2;


    public override void Shoot(Transform muzzle, BulletSO bullet)
    {
        CheckShooting(this);

        //shoot

        int tempBulletsAmount = bulletsToShoot;

        // if odd reduce to make it even
        bool isOdd = tempBulletsAmount % 2 != 0;
        float degBulletsAngle;
        if(isOdd)
        {
            tempBulletsAmount--;
            degBulletsAngle = DegShotgunAngle / tempBulletsAmount;
        }
        else
            degBulletsAngle = DegShotgunAngle / (tempBulletsAmount - 1);

        //getting half of the bullets
        tempBulletsAmount/= 2;

        for (int i = 0; i < tempBulletsAmount; i++)
        {
            float bulletDegAngle = DegShotgunHalfAngle - (degBulletsAngle * i);
            float bulletRadAngle = bulletDegAngle * Mathf.Deg2Rad;
            float x = MathF.Cos(bulletRadAngle);
            float y = MathF.Sin(bulletRadAngle);
            Vector2 bulletDir = new(x, y);
            Vector2 mirroredBulletDir = new(x, -y);

            Vector2 bulletUp = new(-bulletDir.y, bulletDir.x);
            Vector2 mirroredBulletUp = new(-mirroredBulletDir.y, mirroredBulletDir.x);

            Quaternion bulletRot = muzzle.rotation * Quaternion.LookRotation(Vector3.forward, bulletUp);
            Quaternion mirroredBulletRot = muzzle.rotation * Quaternion.LookRotation(Vector3.forward, mirroredBulletUp);

            Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, bulletRot);
            InitBullet(tempBullet);
            onShotFired?.Invoke(transform);
            Destroy(tempBullet.gameObject, 1f);

            Bullet mirroredTempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, mirroredBulletRot);
            InitBullet(mirroredTempBullet);
            onShotFired?.Invoke(transform);
            Destroy(mirroredTempBullet.gameObject, 1f);
        }
        
        if(isOdd)
        {
            Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, muzzle.rotation);
            InitBullet(tempBullet);
            onShotFired?.Invoke(transform);
            Destroy(tempBullet.gameObject, 1f);
        }
        
        fireRateTimer = 0;
        anim.SetTrigger("isShooting");
        bulletsNum--;
        weaponryText.updateAmmo(bulletsNum.ToString());
        //anim.Play("Shoot");

        //recoil
        lastRecoil = recoilTime;

        onShotgunPump?.Invoke(transform);

        //check ammo
        if (bulletsNum == 0)
        {
            canShoot = false;
            Reload(this);
        }
    }

    public override void UpgradeStats()
    {
        base.UpgradeStats();
        bulletsToShoot++;
    }
}
