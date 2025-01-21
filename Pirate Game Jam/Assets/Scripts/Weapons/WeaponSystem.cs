using System.Collections;
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

    public abstract void Shoot(Transform muzzle, BulletSO bullet); //shoot method called

    public WeaponDisplay weaponryText;

    //timer for reloading weapons when running out of ammo

    public void Reload() => CoroutineManager.Instance.StartingCoroutine(ReloadCoroutine());

    public IEnumerator ReloadCoroutine()
    {
        amountOfCoroutines++;
        Debug.Log("Coroutine N: " + amountOfCoroutines + " Started");
        float remainTime = reloadTime;
        weaponryText.updateAmmo("Reloading");

        while (remainTime > 0)
        {
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
