using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text currentWeapon;
    [SerializeField] private TMP_Text bulletsLeft;

    public void updateWeapon(string weapon)
    {
        currentWeapon.text = weapon;
    }
    public void updateAmmo(string Ammo)
    {
        bulletsLeft.text = Ammo;
    }
}
