using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public abstract class WeaponSystem : MonoBehaviour
{
    internal int initialBulletNum;
    public int bulletsNum;
    public float reloadTime;
    public float bulletSpeed = 10f;
    internal bool canShoot = true;
    internal byte weaponIndex;
    int amountOfCoroutines = 0;
    [SerializeField] protected BulletSO bullet;
    internal bool isReloading = false;

    public abstract void Shoot(Transform muzzle, BulletSO bullet); //shoot method called

    public WeaponDisplay weaponryText;

    //dictionay to keep thack of my coroutines 
    public Dictionary<WeaponSystem, Coroutine> myRunningCoroutines = new Dictionary<WeaponSystem, Coroutine>();
    //timer for reloading weapons when running out of ammo

    public void Reload(WeaponSystem weapon)
    {
        if (!weapon.isReloading)
        {
            Coroutine coro = CoroutineManager.Instance.StartingCoroutine(ReloadCoroutine());
            weapon.isReloading = true;
            myRunningCoroutines[weapon] = coro;
        }      
    }

    public void CheckShooting(WeaponSystem weapon)
    {
        weapon.isReloading = false;
        if (myRunningCoroutines.ContainsKey(weapon) && weapon.bulletsNum > 0)
        {
            if (myRunningCoroutines.TryGetValue(weapon, out Coroutine value))
            {
                CoroutineManager.Instance.StoppingCoroutine(value);
                myRunningCoroutines.Remove(weapon);
                weaponryText.disablingLoadingPanels(weaponIndex);
            }
        }
    }
    public IEnumerator ReloadCoroutine()
    {

        amountOfCoroutines++;
        Debug.Log("Coroutine N: " + amountOfCoroutines + " Started");
        float remainTime = reloadTime;
        weaponryText.updateAmmo("Reloading");

        while (remainTime > 0)
        {
            isReloading = true;
            remainTime -= Time.deltaTime;
            //Debug.Log(Mathf.Ceil(remainTime).ToString());
            weaponryText.loadingInfo(weaponIndex, Mathf.Ceil(remainTime).ToString());

            yield return null;
        }

        weaponryText.disablingLoadingPanels(weaponIndex);
        weaponryText.updateAmmo(initialBulletNum.ToString());
        Debug.Log("Coroutine N: " + amountOfCoroutines + " Finished");
        canShoot = true;
        bulletsNum = initialBulletNum;

    }

    private void Awake()
    {

    }

}
