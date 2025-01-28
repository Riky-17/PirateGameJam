using System.Collections.Generic;
using UnityEngine;

public class LivingGunBoss : Boss
{
    [SerializeField] List<LivingGunBossGun> weapons;
    LivingGunBossGun currentWeapon;

    float weaponTime = 15f;
    float weaponTimer;

    protected override void InitBoss()
    {
        currentWeapon = weapons[0];
    }

    protected override void Update()
    {
        ColorFlash();
        TakeAim();

        if(weaponTimer < weaponTime)
        {
            weaponTimer+= Time.deltaTime;
            currentWeapon.Shoot();
        }
        else
        {
            weaponTimer = 0;
            SwitchWeapon();
        }
    }

    public override void TakeAim()
    {
        float x = player.transform.position.x - transform.position.x;
        if(x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.identity;

        base.TakeAim();
    }


    void SwitchWeapon()
    {
        int randomWeaponIndex = Random.Range(0, weapons.Count);
        if(weapons[randomWeaponIndex] == currentWeapon)
            return;

        LivingGunBossGun nextWeapon = weapons[randomWeaponIndex];
        nextWeapon.gameObject.SetActive(true);
        nextWeapon.ChangeWeaponColor(currentWeapon.WeaponColorSprite());
        currentWeapon.ChangeWeaponColor(Color.white);
        currentWeapon.gameObject.SetActive(true);
        currentWeapon = nextWeapon;
    } 

    protected override void LoadNextScene()
    {
        //load Credits Scene
    }

    protected override void UpdateColor(Color color)
    {
        currentWeapon.ChangeWeaponColor(color);
    }
}
