using System.Collections;
using UnityEngine;

public class AssaultScript : WeaponSystem
{
    private bool isShooting = true;
    private int sprayAngle = 20;

    protected override void OnEnable()
    {
        base.OnEnable();
        isShooting = true;
    }

    public override void Shoot(Transform muzzle, BulletSO bullet)
    {
        if (PanelsManager.canReadInput)
        {

            if (Input.GetMouseButton(0) && !Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (canShoot && bulletsNum > 0)
                {
                    //shoot
                   
                    if (isShooting)
                    {
                        CheckShooting(this);
                        int inaccuracy = Random.Range(-sprayAngle, sprayAngle); ;
                        Quaternion bulletSprite = Quaternion.Euler(0f, 0f, inaccuracy);
                        Bullet tempBullet = Instantiate(bullet.bulletPrefab, muzzle.position, transform.rotation * bulletSprite);
                        //initializing the bullet script
                        tempBullet.Init(bullet.speed, pm.gameObject.layer, bullet.damage);
                        
                        //fire the shooting event
                        onShotFired?.Invoke(transform);

                        bulletsNum--;
                        weaponryText.updateAmmo(bulletsNum.ToString());
                        StartCoroutine(FireRate());
                        Destroy(tempBullet.gameObject, 2f);
                        anim.SetTrigger("isShooting");
                        //recoil
                        lastRecoil = recoilTime;
                    }

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

    IEnumerator FireRate()
    {
        isShooting = false;
        yield return new WaitForSeconds(fireRate);
        isShooting = true;
    }
}
