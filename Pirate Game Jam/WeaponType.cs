using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assembly_CSharp
{
    public abstract class WeaponType : MonoBehaviour
    {
        public int bulletsNum;
        public float reloadTime;
        public bool canShoot = true;

        public abstract void Shoot();
        public abstract IEnumerator reloadingSpeed(float loadCooldown);
        public abstract void weaponAmmo();


    }

    public class AssaultRifle : WeaponType
    {
        public override void weaponAmmo()
        {
            bulletsNum = 30;
            reloadTime = 5f;
        }

        public override void Shoot()
        {
            if(canShoot && bulletsNum > 0)
            {
                bulletsNum--;
            }
            else if (bulletsNum <= 0)
            {
                StartCoroutine(reloadingSpeed(reloadTime));
                canShoot = false;
            }
        }
        public override IEnumerator reloadingSpeed(float loadCooldown) 
        {
            while(loadCooldown > 0)
            {
                loadCooldown += 
            }
            return null;
        }

    }

    public class GrenadeLauncher : WeaponType
    {
        public override void weaponAmmo()
        {
            bulletsNum = 5;
            reloadTime = 10f;
        }
        public override void Shoot()
        {
            if (canShoot && bulletsNum > 0)
            {
                StartCoroutine(reloadingSpeed(reloadTime));
                canShoot = false;
            }
            else if (bulletsNum <= 0)
            {
                StartCoroutine(reloadingSpeed(reloadTime));
            }
        }
        public override IEnumerator reloadingSpeed(float loadCooldown)
        {
            return null;
        }

    }
    public class Shotgun : WeaponType
    {
        public override void weaponAmmo()
        {
            bulletsNum = 8;
            reloadTime = 5f;
        }
        public override void Shoot()
        {
            if (canShoot && bulletsNum > 0)
            {
                StartCoroutine(reloadingSpeed(reloadTime));
                canShoot = false;
            }
            else if(bulletsNum <= 0)
            {
                StartCoroutine(reloadingSpeed(reloadTime));
            }
        }
        public override IEnumerator reloadingSpeed(float loadCooldown)
        {
            return null;
        }

    }
    public class Marksman : WeaponType
    {
        public override void weaponAmmo()
        {
            bulletsNum = 10;
            reloadTime = 3f;
        }
        public override void Shoot()
        {
            if (canShoot && bulletsNum > 0)
            {
                StartCoroutine(reloadingSpeed(reloadTime));
                canShoot = false;
            }
            else if (bulletsNum <= 0)
            {
                StartCoroutine(reloadingSpeed(reloadTime));
            }
        }
        public override IEnumerator reloadingSpeed(float loadCooldown)
        {
            return null;
        }

    }
}
