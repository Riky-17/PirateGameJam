using UnityEngine;

public class GrenadeScript : WeaponSystem
{
    public override void Shoot(Transform muzzle, BulletSO bullet)
    {
        //shoot
        CheckShooting(this);
        Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, transform.rotation);
        //initializing the bullet script
        InitBullet(tempBullet);
        fireRateTimer = 0;

        bulletsNum--;
        weaponryText.updateAmmo(bulletsNum.ToString());

        Destroy(tempBullet, 2f);
        anim.SetTrigger("isShooting");
        //recoil
        lastRecoil = recoilTime;

        //fire the shooting event
        onShotFired?.Invoke(transform);

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
        reloadTime *= .95f;
    }
}
