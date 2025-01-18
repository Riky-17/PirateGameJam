using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class WeaponSystem : MonoBehaviour
{
    internal int initialBulletNum;
    public int bulletsNum;
    public float reloadTime;
    internal bool canShoot = true;
    public float bulletSpeed = 10f;

    public abstract void Shoot(Transform muzzle, GameObject bullet); //shoot method called

    //timer for reloading weapons when running out of ammo
    public void ManualReload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("ManualReloadStarted");
        }
    }
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

    private void Awake() { }
    
}
