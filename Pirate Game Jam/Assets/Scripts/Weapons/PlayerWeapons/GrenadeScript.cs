using UnityEngine;

public class GrenadeScript : WeaponSystem
{
    public override void Shoot(Transform muzzle, BulletSO bullet)
    {
        if (Input.GetMouseButtonDown(0) && PanelsManager.canReadInput)
        {
            if (canShoot && bulletsNum > 0)
            {
                //shoot
                CheckShooting(this);
                Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, transform.rotation);
                //initializing the bullet script
                tempBullet.Init(bullet.speed, pm.gameObject.layer, bullet.damage);

                bulletsNum--;
                weaponryText.updateAmmo(bulletsNum.ToString());

                Destroy(tempBullet, 2f);
                anim.SetTrigger("isShooting");
                //recoil
                lastRecoil = recoilTime;

                //check ammo
                if (bulletsNum == 0)
                {
                    canShoot = false;
                    Reload(this);
                }

            }
        }
    }
}
