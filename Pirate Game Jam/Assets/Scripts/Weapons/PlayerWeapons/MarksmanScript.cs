using UnityEngine;

public class MarksmanScript : WeaponSystem
{   
    public override void Shoot(Transform muzzle, BulletSO bullet)
    {
        if (Input.GetMouseButtonDown(0) && PanelsManager.canReadInput)
        {

            if (canShoot && bulletsNum > 0)
            {
                CheckShooting(this);
                //shoot
                Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, transform.rotation);
                //initializing the bullet script
                tempBullet.Init(bullet.speed, pm.gameObject.layer, bullet.damage);

                bulletsNum--;
                weaponryText.updateAmmo(bulletsNum.ToString());

                Destroy(tempBullet, 2f);
                anim.Play("Shoot");

                //recoil
                lastRecoil = recoilTime;
                
                //fire the shooting event
                onShotFired?.Invoke(transform);
                
                //check ammo
                if (bulletsNum <= 0)
                {
                    canShoot = false;
                    Reload(this);
                }

            }
        }
    }
}
