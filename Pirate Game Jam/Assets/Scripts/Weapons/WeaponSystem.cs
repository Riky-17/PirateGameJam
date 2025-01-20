using System.Collections;
using UnityEngine;

/*
public class AssaultRifle : WeaponSystem
{
    private void Awake()
    {
        bulletsNum = 30;
        initialBulletNum = bulletsNum;
        reloadTime = 5f;
    }

    public override void Shoot(Transform muzzle, GameObject bullet)
    {
        if (canShoot && bulletsNum > 0)
        {
            GameObject tempBullet = Instantiate(bullet, muzzle.position, Quaternion.identity);
            tempBullet.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed * Time.deltaTime);
            bulletsNum--;
            Destroy(tempBullet, 2f);
        }
        else if (bulletsNum <= 0)
        {
            StartCoroutine(ReloadingSpeed(reloadTime));
            canShoot = false;
        }
    }

}
public class GrenadeLauncher : WeaponSystem
{
    private void Awake()
    {
        bulletsNum = 5;
        initialBulletNum = bulletsNum;
        reloadTime = 10f;
    }


    public override void Shoot(Transform muzzle, GameObject grenade)
    {
        if (canShoot && bulletsNum > 0)
        {
            GameObject tempGrenade = Instantiate(grenade, muzzle.position, Quaternion.identity);
            tempGrenade.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed * Time.deltaTime);
            Destroy(tempGrenade, 2f);
            bulletsNum--;
        }
        else if (bulletsNum <= 0)
        {
            StartCoroutine(ReloadingSpeed(reloadTime));
            canShoot = false;
        }
    }
}
*/


