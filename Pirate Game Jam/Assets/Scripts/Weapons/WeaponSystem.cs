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

    public WeaponDisplay weaponaryText;

    //timer for reloading weapons when running out of ammo

    public void Reload()
    {

        CourutineManager.Instance.StartingCoroutine(reloadCourutine());
    }
    public IEnumerator reloadCourutine()
    {
        amountOfCoroutines++;
        Debug.Log("Courutine N: " + amountOfCoroutines + " Started");
        float remainTime = reloadTime;
        weaponaryText.updateAmmo("Reloading");
        while (remainTime > 0)
        {
            remainTime -= Time.deltaTime;
            //Debug.Log(Mathf.Ceil(remainTime).ToString());
            weaponaryText.loadingInfo(weaponIndex,(Mathf.Ceil(remainTime).ToString()));
            yield return null;
        }
        weaponaryText.disablingLoadingPanels(weaponIndex);
        weaponaryText.updateAmmo(initialBulletNum.ToString());
        Debug.Log("Courutine N: " + amountOfCoroutines + " Finished");
        canShoot = true;
        bulletsNum = initialBulletNum;

    }
    private void Awake()
    {

    }

}
