using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public abstract class WeaponSystem : MonoBehaviour
{
    internal int initialBulletNum;
    public int bulletsNum;
    public float reloadTime;
    public bool canShoot = true;
    public float bulletSpeed = 10f;

    public abstract void Shoot(Transform muzzle, GameObject bullet); //shoot method called

    internal WeaponDisplay weaponaryText;

    //timer for reloading weapons when running out of ammo
    public void ManualReload(float loadCooldown)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("ManualReloadStarted");
            StartCoroutine(ReloadingSpeed(loadCooldown));
        }
    }
    public IEnumerator ReloadingSpeed(float loadCooldown)
    {
        weaponaryText.updateAmmo("Reloading");
        while (loadCooldown > 0)
        {
            loadCooldown -= Time.deltaTime;
            yield return null;
        }
        canShoot = true;
        bulletsNum = initialBulletNum;
        weaponaryText.updateAmmo("Full Ammo!");

        //add in canvas the visual description of this 

    }

    private void Awake() 
    {       
        
    }
    
}
