using System;
using UnityEngine;

public class ShotgunScript : WeaponSystem
{
    public static Action<Transform> onShotgunPump;

    public override void Shoot(Transform muzzle, BulletSO bullet)
    {
        if (Input.GetMouseButtonDown(0) && PanelsManager.canReadInput)
        {

            if (canShoot && bulletsNum > 0)
            {
                CheckShooting(this);
                //shoot
                for (int bulletsFriction = 30; bulletsFriction >= -30; bulletsFriction-= 15)
                {
                    Quaternion tempRotation = Quaternion.Euler(0f, 0f, bulletsFriction);
                    Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, muzzle.rotation * tempRotation);
                    //initializing the bullet script
                    tempBullet.Init(bullet.speed, pm.gameObject.layer, bullet.damage);
                    
                    //fire the shooting event
                    onShotFired?.Invoke(transform);

                    Destroy(tempBullet.gameObject, 1f);
                }
                
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

        }
    }
}
