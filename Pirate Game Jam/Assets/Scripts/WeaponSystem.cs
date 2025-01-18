using System.Collections;
using UnityEngine;

public abstract class WeaponSystem : MonoBehaviour
{
    internal int initialBulletNum;
    public int bulletsNum;
    public float reloadTime;
    internal bool canShoot = true;
    public float bulletSpeed = 10f;

    public abstract void Shoot(Transform muzzle, GameObject bullet); //shoot method called

    //timer for reloading weapons when running out of ammo
    public IEnumerator ReloadingSpeed(float loadCooldown)
    {
        while (loadCooldown > 0)
        {
            loadCooldown -= Time.deltaTime;
            yield return null;
        }
        canShoot = true;
        bulletsNum = initialBulletNum;
        //add in canvas the visual description of this 

    }

    public abstract void WeaponAmmo(); //initializing variables in weapons
}

public class AssaultRifle : WeaponSystem
{
    public override void WeaponAmmo()
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
    public override void WeaponAmmo()
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

public class Shotgun : WeaponSystem
{
    public override void WeaponAmmo()
    {
        bulletsNum = 8;
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

public class Marksman : WeaponSystem
{
    public override void WeaponAmmo()
    {
        bulletsNum = 10;
        initialBulletNum = bulletsNum;
        reloadTime = 3f;
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

