using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text currentWeapon;
    [SerializeField] private TMP_Text bulletsLeft;

    [SerializeField] private Image[] weaponsChosen;
    [SerializeField] private TMP_Text[] weaponsInfo;
    [SerializeField] private GameObject[] loadingPanels;


    //information for the panel -> temporary solution 
    [SerializeField] private TMP_Text damage;
    [SerializeField] private TMP_Text fireRate;
    [SerializeField] private TMP_Text reloadTime;
    [SerializeField] private TMP_Text maxAmmo;
    private void Awake()
    {
        foreach (var weapon in weaponsChosen)
        {
            weapon.gameObject.SetActive(false);
        }

        weaponsChosen[0].gameObject.SetActive(true);

        //turn it off the panels, setting them back on when loading 
        foreach(GameObject panel in loadingPanels)
        {
            panel.gameObject.SetActive(false);
        }
    }
    public void updateWeapon(string weapon)
    {
        currentWeapon.text = weapon;
    }
    public void updateAmmo(string Ammo)
    {
        bulletsLeft.text = Ammo;
    }
    public void UpdateWeaponChosen(byte number)
    {
        foreach (var weapon in weaponsChosen) 
        { 
            weapon.gameObject.SetActive(false);
        }
        weaponsChosen[number].gameObject.SetActive(true);
    }
    public void PauseWeaponInfo(string damage, string fireRate, string reloadSpeed, string maxAmmo)
    {
        weaponsInfo[0].text = damage;
        weaponsInfo[1].text = fireRate;
        weaponsInfo[2].text = reloadSpeed;
        weaponsInfo[3].text = maxAmmo;
    }

    public void loadingInfo(byte weaponIndex, string secondsLeft)
    {
        loadingPanels[weaponIndex - 1].gameObject.SetActive(true);
        loadingPanels[weaponIndex - 1].GetComponentInChildren<TMP_Text>().text = secondsLeft.ToString();
    }
    public void disablingLoadingPanels(byte weaponIndex)
    {
        loadingPanels[weaponIndex - 1].gameObject.SetActive(false);
    }

    public void OnClickDamage(string damage)       
    {
        this.damage.text = damage;  
    }
    public void OnClickFireRate(string fireRate)
    {
        this.fireRate.text = fireRate;
    }
    public void OnClickReloadSPeed(string reloadTime)
    {
        this.reloadTime.text = reloadTime;
    }
    public void OnClickMaxAmmo(string maxAmmo)
    {
        this.maxAmmo.text = maxAmmo;
    }

    public void onClickWeaponInfo(WeaponSystem obj)
    {
       reloadTime.text = obj.ReloadTime.ToString();
       maxAmmo.text = obj.InitialBulletNum.ToString();       
    }
}
